using System;
using System.Collections.Generic;

using System.Text;
using System.Runtime.InteropServices;
using System.Collections;

namespace GvoHelper
{
    class WinAPIHook
    {
        [DllImport("ws2_32.dll")]
        static extern int send(int s, byte[] buf, int len, int flag);

        [DllImport("ws2_32.dll")]
        static extern int recv(int s, byte[] buf, int len, int flag);

        [DllImport("Kernel32.dll", EntryPoint = "GetModuleHandleA")]
        static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("Kernel32.dll")]
        static extern bool VirtualProtect(
            IntPtr lpAddress,
            int dwSize,
            int flNewProtect,
            ref int lpflOldProtect
            );

        [DllImport("Kernel32.dll", EntryPoint = "lstrcpynA")]
        static extern IntPtr lstrcpyn(
            byte[] lpString1,
            byte[] lpString2,
            int iMaxLength
            );

        [DllImport("Kernel32.dll")]
        static extern IntPtr GetProcAddress(
            IntPtr hModule,
            string lpProcName
            );

        [DllImport("Kernel32.dll")]
        static extern bool FreeLibrary(
            IntPtr hModule
            );

        const int PAGE_EXECUTE_READWRITE = 0x40;

        IntPtr ProcAddress;
        IntPtr OldAddress;
        int lpflOldProtect = 0;
        byte[] OldEntry = new byte[5];
        byte[] NewEntry = new byte[5];

        public delegate int sendCallback(int s, IntPtr buf, int len, int flag);
        public delegate int recvCallback(int s, IntPtr buf, int len, int flag);

        //public bool APIHOOK() { }

        //public bool APIHOOK(string ModuleName, string ProcName, IntPtr lpAddress)
        //{
        //    return Install(ModuleName, ProcName, lpAddress);
        //}

        public bool Install(string ModuleName, string ProcName, IntPtr lpAddress)
        {
            IntPtr hModule = GetModuleHandle(ModuleName); //取模块句柄   
            if (hModule == IntPtr.Zero) return false;
            ProcAddress = GetProcAddress(hModule, ProcName); //取入口地址   
            if (ProcAddress == IntPtr.Zero) return false;
            if (!VirtualProtect(ProcAddress, 5, PAGE_EXECUTE_READWRITE, ref lpflOldProtect)) return false; //修改内存属性   
            Marshal.Copy(ProcAddress, OldEntry, 0, 5); //读取前5字节   
            NewEntry = AddBytes(new byte[1] { 233 }, BitConverter.GetBytes((Int32)((Int32)lpAddress - (Int32)ProcAddress - 5))); //计算新入口跳转   
            Marshal.Copy(NewEntry, 0, ProcAddress, 5); //写入前5字节   
            OldEntry = AddBytes(OldEntry, new byte[5] { 233, 0, 0, 0, 0 });
            OldAddress = lstrcpyn(OldEntry, OldEntry, 0); //取变量指针   
            Marshal.Copy(BitConverter.GetBytes((double)((Int32)ProcAddress - (Int32)OldAddress - 5)), 0, (IntPtr)(OldAddress.ToInt32() + 6), 4); //保存JMP   
            FreeLibrary(hModule); //释放模块句柄   
            return true;
        }

        public void Suspend()
        {
            Marshal.Copy(OldEntry, 0, ProcAddress, 5);
        }

        public void Continue()
        {
            Marshal.Copy(NewEntry, 0, ProcAddress, 5);
        }

        public bool Uninstall()
        {
            if (ProcAddress == IntPtr.Zero) return false;
            Marshal.Copy(OldEntry, 0, ProcAddress, 5);
            ProcAddress = IntPtr.Zero;
            return true;
        }

        static byte[] AddBytes(byte[] a, byte[] b)
        {
            ArrayList retArray = new ArrayList();
            for (int i = 0; i < a.Length; i++)
            {
                retArray.Add(a[i]);
            }
            for (int i = 0; i < b.Length; i++)
            {
                retArray.Add(b[i]);
            }
            return (byte[])retArray.ToArray(typeof(byte));
        }

        public int sendProc(int s, IntPtr buf, int len, int flag)
        {
            byte[] buffer = new byte[len];
            Marshal.Copy(buf, buffer, 0, len); //读封包数据,读取后可进行条件修改,拦截,转发等,记得处理后调用发送   
            Suspend(); //暂停拦截，转交系统调用   
            int ret = send(s, buffer, len, flag); //发送数据，此处可进行拦截
            Continue(); //恢复HOOK   
            return ret;
        }

        public int toProc(int s, IntPtr buf, int len, int flag)
        {
            byte[] buffer = new byte[len];
            Marshal.Copy(buf, buffer, 0, len); //读封包数据 
            Suspend(); //暂停拦截，转交系统调用   
            int ret = recv(s, buffer, len, flag); //发送数据，此处可对包进行处理操作
            Continue(); //恢复HOOK   
            return ret;
        }
    }
}