using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace GvoHelper
{
    class AsmClassLibrary
    {
        [DllImport("kernel32.dll", EntryPoint = "CloseHandle")]
        public static extern int CloseHandle(int hObject);

        [DllImport("kernel32.dll")]
        public static extern Int32 WriteProcessMemory(IntPtr hProcess,
        IntPtr lpBaseaddrss, [In, Out] byte[] buffer, int size, out IntPtr lpNumberOfBytesWritten);

        [DllImport("kernel32.dll")]
        public static extern Int32 WriteProcessMemory(int hProcess,
        int lpBaseaddrss,
        byte[] buffer,
        int size,
        int lpNumberOfBytesWritten);

        [DllImport("kernel32", EntryPoint = "CreateRemoteThread")]
        public static extern int CreateRemoteThread(
            int hProcess,
            int lpThreadAttributes,
            int dwStackSize,
            int lpStartaddrss,
            int lpParameter,
            int dwCreationFlags,
            ref int lpThreadId);

        [DllImport("Kernel32.dll")]
        public static extern System.Int32 VirtualAllocEx(
         System.IntPtr hProcess,
         System.Int32 lpaddrss,
         System.Int32 dwSize,
         System.Int16 flAllocationType,
         System.Int16 flProtect);

        [DllImport("Kernel32.dll")]
        public static extern System.Int32 VirtualAllocEx(
        int hProcess,
        int lpaddrss,
        int dwSize,
        int flAllocationType,
        int flProtect);

        [DllImport("Kernel32.dll")]
        public static extern System.Int32 VirtualFreeEx(
        int hProcess,
        int lpaddrss,
        int dwSize,
        int flAllocationType);

        [DllImport("kernel32.dll", EntryPoint = "OpenProcess")]
        public static extern int OpenProcess(
        int dwDesiredAccess,
        int bInheritHandle,
        int dwProcessId);

        private const int PAGE_EXECUTE_READWRITE = 0x4;
        private const int MEM_COMMIT = 4096;
        private const int MEM_RELEASE = 0x8000;
        private const int MEM_DECOMMIT = 0x4000;
        private const int PROCESS_ALL_ACCESS = 0x1F0FFF;
        private const int PROCESS_CREATE_THREAD = 0x2;
        private const int PROCESS_VM_OPERATION = 0x8;
        private const int PROCESS_VM_WRITE = 0x20;
        public string Asmcode="";

        private Object thisLock = new Object();

        private string hex(int addrss)
        {
            string str = addrss.ToString("X");
            return str;
        }

        public string intTohex(int value, int num)
        {
            string str1;
           string str2 = "";
            str1 = "0000000" + this.hex(value);
            str1 = str1.Substring(str1.Length - num,num);
            for (int i = 0; i < str1.Length/2; i++)
            {
                str2 = str2 + str1.Substring(str1.Length - 2 - 2 * i, 2);
            }
            return str2;
        }

        public void SUB_ESP(int addr)
        {
            if ((addr <= 127) && (addr >= -128))
            {
                this.Asmcode = this.Asmcode + "83EC" + intTohex(addr, 2);
            }
            else
            {
                this.Asmcode = this.Asmcode + "81EC" + intTohex(addr, 8);
            }
        }
       
        #region ADD
        public void Add_EAX_EDX()
        {
            this.Asmcode = this.Asmcode + "03C2";
        }

        public void Add_EBX_EAX()
        {
            this.Asmcode = this.Asmcode + "03D8";
        }

        public void Add_EAX_DWORD_Ptr(int addr)
        {
            this.Asmcode = this.Asmcode + "0305" + intTohex(addr, 8);
        }

        public void Add_EBX_DWORD_Ptr(int addr)
        {
            this.Asmcode = this.Asmcode + "031D" + intTohex(addr, 8);
        }

        public void Add_EBP_DWORD_Ptr(int addr)
        {
            this.Asmcode = this.Asmcode + "032D" + intTohex(addr, 8);
        }

        public void Add_EAX(int addr)
        {
            this.Asmcode = this.Asmcode + "05" + intTohex(addr, 8);
        }

        public void Add_EBX(int addr)
        {
            if((addr<=127)&&(addr>=-128))
            this.Asmcode = this.Asmcode + "83C3" + intTohex(addr, 2);
            else
            this.Asmcode = this.Asmcode + "81C3" + intTohex(addr, 8);
        }

        public void Add_ECX(int addr)
        {
            if((addr<=127)&&(addr>=-128))
            this.Asmcode = this.Asmcode + "83C1" + intTohex(addr, 2);
            else
            this.Asmcode = this.Asmcode + "81C1" + intTohex(addr, 8);
        }

        public void Add_EDX(int addr)
        {
            if((addr<=127)&&(addr>=-128))
            this.Asmcode = this.Asmcode + "83C2" + intTohex(addr, 2);
            else
            this.Asmcode = this.Asmcode + "81C2" + intTohex(addr, 8);
        }

        public void Add_ESI(int addr)
        {
            if ((addr <= 127) && (addr >= -128))
            this.Asmcode = this.Asmcode + "83C6" + intTohex(addr, 2);
            else
            this.Asmcode = this.Asmcode + "81C6" + intTohex(addr, 8);
        }

        public void Add_ESP(int addr)
        {
            if ((addr <= 127) && (addr >= -128))
            this.Asmcode = this.Asmcode + "83C4" + intTohex(addr, 2);
            else
           this.Asmcode = this.Asmcode + "81C4" + intTohex(addr, 8);
        }
        #endregion 

        public void Nop()
        {
            this.Asmcode = this.Asmcode + "90";
        }

        public void Retaddr(int addr)
        {
            this.Asmcode = this.Asmcode + intTohex(addr, 4);
        }

        public void Ret()
        {
            this.Asmcode = this.Asmcode + "C3";
        }

        public void IN_AL_DX()
        {
            this.Asmcode = this.Asmcode + "EC";
        }

        public void TEST_EAX_EAX()
        {
            this.Asmcode = this.Asmcode + "85C0";
        }

        public void Leave()
        {
            this.Asmcode = this.Asmcode + "C9";
        }

        public void Pushad()
        {
            this.Asmcode = this.Asmcode + "60";
        }

        public void Popad()
        {
            this.Asmcode = this.Asmcode + "61";
        }

        public void RunAsm(int pid)
        {
            int hwnd, addr, threadhwnd;
            byte[] Asm = this.AsmChangebytes(this.Asmcode);
            if (pid != 0)
            {
                hwnd = OpenProcess(PROCESS_ALL_ACCESS | PROCESS_CREATE_THREAD | PROCESS_VM_WRITE, 0, pid);
                if (hwnd != 0)
                {
                    addr = VirtualAllocEx(hwnd, 0, Asm.Length, MEM_COMMIT, PAGE_EXECUTE_READWRITE);
                    WriteProcessMemory(hwnd, addr, Asm, Asm.Length, 0);
                    threadhwnd = CreateRemoteThread(hwnd, 0, 0, addr, 0, 0, ref pid);
                    VirtualFreeEx(hwnd, addr, Asm.Length, MEM_RELEASE);
                    CloseHandle(threadhwnd);
                    CloseHandle(hwnd);
                }
            }
            this.Asmcode = "";
        }

        private byte[] AsmChangebytes(string asmPram)
        {
            byte[] reAsmCode = new byte[asmPram.Length / 2];
            for (int i = 0; i < reAsmCode.Length; i++)
            {
                reAsmCode[i] = Convert.ToByte(Int32.Parse(asmPram.Substring(i * 2, 2), System.Globalization.NumberStyles.AllowHexSpecifier));

            }
            return reAsmCode;
        }

        #region mov

        public void Movzx_EDI_AX()
        {
            this.Asmcode = this.Asmcode + "0FB7F8";
        }

        public void Mov_DWORD_Ptr_EAX_ADD(int addr, int addr1)
        {
            if ((addr <= 127) && (addr >= -128))
            {
                this.Asmcode = this.Asmcode + "C740" + intTohex(addr, 2) + intTohex(addr1, 8);
            }
            else
            {
                this.Asmcode = this.Asmcode + "C780" + intTohex(addr, 8) + intTohex(addr1, 8);
            }
        }

        public void Mov_BYTE_Ptr_ESP_ADD(int addr, int addr1)
        {
            if ((addr <= 127) && (addr >= -128))
            {
                this.Asmcode = this.Asmcode + "C64424" + intTohex(addr, 2) + intTohex(addr1, 2);
            }
            else
            {
                this.Asmcode = this.Asmcode + "C68424" + intTohex(addr, 8) + intTohex(addr1, 2);
            }
        }

        public void Mov_DWORD_Ptr_ESP_ADD(int addr,int addr1)
        {
            if ((addr <= 127) && (addr >= -128))
            {
                this.Asmcode = this.Asmcode + "C74424" + intTohex(addr, 2) + intTohex(addr1, 8);
            }
            else
            {
                this.Asmcode = this.Asmcode + "C78424" + intTohex(addr, 8) + intTohex(addr1, 8);
            }
        }

        public void Mov_WORD_Ptr_ESP_ADD_AX(int addr)
        {
            this.Asmcode = this.Asmcode + "66" + "894424" + intTohex(addr, 2);
        }

        public void Mov_DWORD_Ptr_ESP_ADD_EAX(int addr)
        {
            if ((addr <= 127) && (addr >= -128))
            {
                this.Asmcode = this.Asmcode + "894424" + intTohex(addr, 2);
            }
            else
            {
                this.Asmcode = this.Asmcode + "898424" + intTohex(addr, 8);
            }
        }

        public void Mov_DWORD_Ptr_ESI_ADD_EAX(int addr)
        {
            if ((addr <= 127) && (addr >= -128))
            {
                this.Asmcode = this.Asmcode + "8946" + intTohex(addr, 2);
            }
        }

        public void Mov_DWORD_Ptr_ESI_ADD_EDX(int addr)
        {
            if ((addr <= 127) && (addr >= -128))
            {
                this.Asmcode = this.Asmcode + "8956" + intTohex(addr, 2);
            }
        }

        public void Mov_DWORD_Ptr_ESP(int addr)
        {
            this.Asmcode = this.Asmcode + "C70424" + intTohex(addr, 8);
        }

        public void Mov_DWORD_Ptr_EAX(int addr)
        {
            this.Asmcode = this.Asmcode + "A3" + intTohex(addr, 8);
        }

        public void Mov_EBX_DWORD_Ptr(int addr)
        {
            this.Asmcode = this.Asmcode + "8B1D" + intTohex(addr, 8);
        }

        public void Mov_ECX_DWORD_Ptr(int addr)
        {
            this.Asmcode = this.Asmcode + "8B0D" + intTohex(addr, 8);
        }

        public void Mov_EAX_DWORD_Ptr(int addr)
        {
            this.Asmcode = this.Asmcode + "A1" + intTohex(addr, 8);
        }

        public void Mov_EDX_DWORD_Ptr(int addr)
        {
            this.Asmcode = this.Asmcode + "8B15" + intTohex(addr, 8);
        }

        public void Mov_ESI_DWORD_Ptr(int addr)
        {
            this.Asmcode = this.Asmcode + "8B35" + intTohex(addr, 8);
        }

        public void Mov_ESP_DWORD_Ptr(int addr)
        {
            this.Asmcode = this.Asmcode + "8B25" + intTohex(addr, 8);
        }

        public void Mov_EBP_DWORD_Ptr(int addr)
        {
            this.Asmcode = this.Asmcode + "8B2D" + intTohex(addr, 8);
        }

        public void Mov_EAX_DWORD_Ptr_EAX(int addr)
        {
            this.Asmcode = this.Asmcode + "8B00";
        }

        public void Mov_EAX_DWORD_Ptr_EAX()
        {
            this.Asmcode = this.Asmcode + "8B00";
        }

        public void Mov_EAX_DWORD_Ptr_EBP()
        {
            this.Asmcode = this.Asmcode + "8B4500";
        }

        public void Mov_EAX_DWORD_Ptr_EBX()
        {
            this.Asmcode = this.Asmcode + "8B03";
        }

        public void Mov_EAX_DWORD_Ptr_ECX()
        {
            this.Asmcode = this.Asmcode + "8B01";
        }

        public void Mov_EAX_DWORD_Ptr_EDX()
        {
            this.Asmcode = this.Asmcode + "8B02";
        }

        public void Mov_EAX_DWORD_Ptr_EDI()
        {
            this.Asmcode = this.Asmcode + "8B07";
        }

        public void Mov_EAX_DWORD_Ptr_ESP()
        {
            this.Asmcode = this.Asmcode + "8B0424";
        }

        public void Mov_EAX_DWORD_Ptr_ESI()
        {
            this.Asmcode = this.Asmcode + "8B06";
        }

        public void Mov_EAX_DWORD_Ptr_EAX_Add(int addr)
        {
            if ((addr <= 127) && (addr >= -128))
            {
                this.Asmcode = this.Asmcode + "8B40" + intTohex(addr, 2);
            }
            else
            {
                this.Asmcode = this.Asmcode + "8B80" + intTohex(addr, 8);
            }
        }

        public void Mov_EAX_DWORD_Ptr_ESP_Add(int addr)
        {
             if((addr<=127)&&(addr>=-128))
                this.Asmcode = this.Asmcode + "8B4424" + intTohex(addr, 2);
            else
                this.Asmcode = this.Asmcode + "8B8424" + intTohex(addr, 8);
        }

        public void Mov_EAX_DWORD_Ptr_EBX_Add(int addr)
        {
             if((addr<=127)&&(addr>=-128))
                this.Asmcode = this.Asmcode + "8B43" + intTohex(addr, 2);
            else
                this.Asmcode = this.Asmcode + "8B83" + intTohex(addr, 8);
        }

        public void Mov_EAX_DWORD_Ptr_ECX_Add(int addr)
        {
            if ((addr <= 127) && (addr >= -128))
                this.Asmcode = this.Asmcode + "8B41" + intTohex(addr, 2);
            else
                this.Asmcode = this.Asmcode + "8B81" + intTohex(addr, 8);
        }

        public void Mov_EAX_DWORD_Ptr_EDX_Add(int addr)
        {
            if ((addr <= 127) && (addr >= -128))
                this.Asmcode = this.Asmcode + "8B42" + intTohex(addr, 2);
            else
                this.Asmcode = this.Asmcode + "8B82" + intTohex(addr, 8);
        }

        public void Mov_EAX_DWORD_Ptr_EDI_Add(int addr)
        {
            if ((addr <= 127) && (addr >= -128))
                this.Asmcode = this.Asmcode + "8B47" + intTohex(addr, 2);
            else
                this.Asmcode = this.Asmcode + "8B87" + intTohex(addr, 8);
        }

        public void Mov_EAX_DWORD_Ptr_EBP_Add(int addr)
        {
            if ((addr <= 127) && (addr >= -128))
                this.Asmcode = this.Asmcode + "8B45" + intTohex(addr, 2);
            else
                this.Asmcode = this.Asmcode + "8B85" + intTohex(addr, 8);
        }

        public void Mov_EAX_DWORD_Ptr_ESI_Add(int addr)
        {
            if ((addr <= 127) && (addr >= -128))
                this.Asmcode = this.Asmcode + "8B46" + intTohex(addr, 2);
            else
                this.Asmcode = this.Asmcode + "8B86" + intTohex(addr, 8);
        }

        public void Mov_EBX_DWORD_Ptr_EAX_Add(int addr)
        {
            if ((addr <= 127) && (addr >= -128))
                this.Asmcode = this.Asmcode + "8B58" + intTohex(addr, 2);
            else
                this.Asmcode = this.Asmcode + "8B98" + intTohex(addr, 8);
        }

        public void Mov_EBX_DWORD_Ptr_ESP_Add(int addr)
        {
            if ((addr <= 127) && (addr >= -128))
                this.Asmcode = this.Asmcode + "8B5C24" + intTohex(addr, 2);
            else
                this.Asmcode = this.Asmcode + "8B9C24" + intTohex(addr, 8);
        }

        public void Mov_EBX_DWORD_Ptr_EBX_Add(int addr)
        {
            if ((addr <= 127) && (addr >= -128))
                this.Asmcode = this.Asmcode + "8B5B" + intTohex(addr, 2);
            else
                this.Asmcode = this.Asmcode + "8B9B" + intTohex(addr, 8);
        }

        public void Mov_EBX_DWORD_Ptr_ECX_Add(int addr)
        {
            if ((addr <= 127) && (addr >= -128))
                this.Asmcode = this.Asmcode + "8B59" + intTohex(addr, 2);
            else
                this.Asmcode = this.Asmcode + "8B99" + intTohex(addr, 8);
        }

        public void Mov_EBX_DWORD_Ptr_EDX_Add(int addr)
        {
            if ((addr <= 127) && (addr >= -128))
                this.Asmcode = this.Asmcode + "8B5A" + intTohex(addr, 2);
            else
                this.Asmcode = this.Asmcode + "8B9A" + intTohex(addr, 8);
        }

        public void Mov_EBX_DWORD_Ptr_EDI_Add(int addr)
        {
            if ((addr <= 127) && (addr >= -128))
                this.Asmcode = this.Asmcode + "8B5F" + intTohex(addr, 2);
            else
                this.Asmcode = this.Asmcode + "8B9F" + intTohex(addr, 8);
        }

        public void Mov_EBX_DWORD_Ptr_EBP_Add(int addr)
        {
            if ((addr <= 127) && (addr >= -128))
                this.Asmcode = this.Asmcode + "8B5D" + intTohex(addr, 2);
            else
                this.Asmcode = this.Asmcode + "8B9D" + intTohex(addr, 8);
        }

        public void Mov_EBX_DWORD_Ptr_ESI_Add(int addr)
        {
            if ((addr <= 127) && (addr >= -128))
                this.Asmcode = this.Asmcode + "8B5E" + intTohex(addr, 2);
            else
                this.Asmcode = this.Asmcode + "8B9E" + intTohex(addr, 8);
        }

        public void Mov_ECX_DWORD_Ptr_EAX_Add(int addr)
        {
            if ((addr <= 127) && (addr >= -128))
                this.Asmcode = this.Asmcode + "8B48" + intTohex(addr, 2);
            else
                this.Asmcode = this.Asmcode + "8B88" + intTohex(addr, 8);
        }

        public void Mov_ECX_DWORD_Ptr_ESP_Add(int addr)
        {
            if ((addr <= 127) && (addr >= -128))
                this.Asmcode = this.Asmcode + "8B4C24" + intTohex(addr, 2);
            else
                this.Asmcode = this.Asmcode + "8B8C24" + intTohex(addr, 8);
        }

        public void Mov_ECX_DWORD_Ptr_EBX_Add(int addr)
        {
            if ((addr <= 127) && (addr >= -128))
                this.Asmcode = this.Asmcode + "8B4B" + intTohex(addr, 2);
            else
                this.Asmcode = this.Asmcode + "8B8B" + intTohex(addr, 8);
        }

        public void Mov_ECX_DWORD_Ptr_ECX_Add(int addr)
        {
            if ((addr <= 127) && (addr >= -128))
                this.Asmcode = this.Asmcode + "8B49" + intTohex(addr, 2);
            else
                this.Asmcode = this.Asmcode + "8B89" + intTohex(addr, 8);
        }

        public void Mov_ECX_DWORD_Ptr_EDX_Add(int addr)
        {
            if ((addr <= 127) && (addr >= -128))
                this.Asmcode = this.Asmcode + "8B4A" + intTohex(addr, 2);
            else
                this.Asmcode = this.Asmcode + "8B8A" + intTohex(addr, 8);
        }

        public void Mov_ECX_DWORD_Ptr_EDI_Add(int addr)
        {
            if ((addr <= 127) && (addr >= -128))
                this.Asmcode = this.Asmcode + "8B4F" + intTohex(addr, 2);
            else
                this.Asmcode = this.Asmcode + "8B8F" + intTohex(addr, 8);
        }

        public void Mov_ECX_DWORD_Ptr_EBP_Add(int addr)
        {
            if ((addr <= 127) && (addr >= -128))
                this.Asmcode = this.Asmcode + "8B4D" + intTohex(addr, 2);
            else
                this.Asmcode = this.Asmcode + "8B8D" + intTohex(addr, 8);
        }

        public void Mov_ECX_DWORD_Ptr_ESI_Add(int addr)
        {
            if ((addr <= 127) && (addr >= -128))
                this.Asmcode = this.Asmcode + "8B4E" + intTohex(addr, 2);
            else
                this.Asmcode = this.Asmcode + "8B8E" + intTohex(addr, 8);
        }

        public void Mov_EDX_DWORD_Ptr_EAX_Add(int addr)
        {
            if ((addr <= 127) && (addr >= -128))
                this.Asmcode = this.Asmcode + "8B50" + intTohex(addr, 2);
            else
                this.Asmcode = this.Asmcode + "8B90" + intTohex(addr, 8);
        }

        public void Mov_EDX_DWORD_Ptr_ESP_Add(int addr)
        {
            if ((addr <= 127) && (addr >= -128))
                this.Asmcode = this.Asmcode + "8B5424" + intTohex(addr, 2);
            else
                this.Asmcode = this.Asmcode + "8B9424" + intTohex(addr, 8);
        }

        public void Mov_EDX_DWORD_Ptr_EBX_Add(int addr)
        {
            if ((addr <= 127) && (addr >= -128))
                this.Asmcode = this.Asmcode + "8B53" + intTohex(addr, 2);
            else
                this.Asmcode = this.Asmcode + "8B93" + intTohex(addr, 8);
        }

        public void Mov_EDX_DWORD_Ptr_ECX_Add(int addr)
        {
            if ((addr <= 127) && (addr >= -128))
                this.Asmcode = this.Asmcode + "8B51" + intTohex(addr, 2);
            else
                this.Asmcode = this.Asmcode + "8B91" + intTohex(addr, 8);
        }

        public void Mov_EDX_DWORD_Ptr_EDX_Add(int addr)
        {
            if ((addr <= 127) && (addr >= -128))
                this.Asmcode = this.Asmcode + "8B52" + intTohex(addr, 2);
            else
                this.Asmcode = this.Asmcode + "8B92" + intTohex(addr, 8);
        }

        public void Mov_EDX_DWORD_Ptr_EDI_Add(int addr)
        {
            if ((addr <= 127) && (addr >= -128))
                this.Asmcode = this.Asmcode + "8B57" + intTohex(addr, 2);
            else
                this.Asmcode = this.Asmcode + "8B97" + intTohex(addr, 8);
        }

        public void Mov_EDX_DWORD_Ptr_EBP_Add(int addr)
        {
            if ((addr <= 127) && (addr >= -128))
                this.Asmcode = this.Asmcode + "8B55" + intTohex(addr, 2);
            else
                this.Asmcode = this.Asmcode + "8B95" + intTohex(addr, 8);
        }

        public void Mov_EDX_DWORD_Ptr_ESI_Add(int addr)
        {
            if ((addr <= 127) && (addr >= -128))
                this.Asmcode = this.Asmcode + "8B56" + intTohex(addr, 2);
            else
                this.Asmcode = this.Asmcode + "8B96" + intTohex(addr, 8);
        }

        public void Mov_ECX_EAX()
        {
            this.Asmcode = this.Asmcode + "8BC8";
        }

        public void Mov_EAX(int addr)
        {
            this.Asmcode = this.Asmcode + "B8" + intTohex(addr, 8);
        }

        public void Mov_EBX(int addr)
        {
            this.Asmcode = this.Asmcode + "BB" + intTohex(addr, 8);
        }

        public void Mov_ECX(int addr)
        {
            this.Asmcode = this.Asmcode + "B9" + intTohex(addr, 8);
        }

        public void Mov_EDX(int addr)
        {
            this.Asmcode = this.Asmcode + "BA" + intTohex(addr, 8);
        }

        public void Mov_ESI(int addr)
        {
            this.Asmcode = this.Asmcode + "BE" + intTohex(addr, 8);
        }

        public void Mov_ESP(int addr)
        {
            this.Asmcode = this.Asmcode + "BC" + intTohex(addr, 8);
        }

        public void Mov_EBP(int addr)
        {
            this.Asmcode = this.Asmcode + "BD" + intTohex(addr, 8);
        }

        public void Mov_EDI(int addr)
        {
            this.Asmcode = this.Asmcode + "BF" + intTohex(addr, 8);
        }

        public void Mov_ESI_DWORD_Ptr_EAX()
        {
            this.Asmcode = this.Asmcode + "8B7020";
        }

        public void Mov_EBX_DWORD_Ptr_EAX()
        {
            this.Asmcode = this.Asmcode + "8B18";
        }

        public void Mov_EBX_DWORD_Ptr_EBP()
        {
            this.Asmcode = this.Asmcode + "8B5D00";
        }

        public void Mov_EBX_DWORD_Ptr_EBX()
        {
            this.Asmcode = this.Asmcode + "8B1B";
        }

        public void Mov_EBX_DWORD_Ptr_ECX()
        {
            this.Asmcode = this.Asmcode + "8B19";
        }

        public void Mov_EBX_DWORD_Ptr_EDX()
        {
            this.Asmcode = this.Asmcode + "8B1A";
        }

        public void Mov_EBX_DWORD_Ptr_EDI()
        {
            this.Asmcode = this.Asmcode + "8B1F";
        }

        public void Mov_EBX_DWORD_Ptr_ESP()
        {
            this.Asmcode = this.Asmcode + "8B1C24";
        }

        public void Mov_EBX_DWORD_Ptr_ESI()
        {
            this.Asmcode = this.Asmcode + "8B1E";
        }

        public void Mov_ECX_DWORD_Ptr_EAX()
        {
            this.Asmcode = this.Asmcode + "8B08";
        }

        public void Mov_ECX_DWORD_Ptr_EBP()
        {
            this.Asmcode = this.Asmcode + "8B4D00";
        }

        public void Mov_ECX_DWORD_Ptr_EBX()
        {
            this.Asmcode = this.Asmcode + "8B0B";
        }

        public void Mov_ECX_DWORD_Ptr_ECX()
        {
            this.Asmcode = this.Asmcode + "8B09";
        }

        public void Mov_ECX_DWORD_Ptr_EDX()
        {
            this.Asmcode = this.Asmcode + "8B0A";
        }

        public void Mov_ECX_DWORD_Ptr_EDI()
        {
            this.Asmcode = this.Asmcode + "8B0F";
        }

        public void Mov_ECX_DWORD_Ptr_ESP()
        {
            this.Asmcode = this.Asmcode + "8B0C24";
        }

        public void Mov_ECX_DWORD_Ptr_ESI()
        {
            this.Asmcode = this.Asmcode + "8B0E";
        }

        public void Mov_EDX_DWORD_Ptr_EAX()
        {
            this.Asmcode = this.Asmcode + "8B10";
        }

        public void Mov_EDX_DWORD_Ptr_EBP()
        {
            this.Asmcode = this.Asmcode + "8B5500";
        }

        public void Mov_EDX_DWORD_Ptr_EBX()
        {
            this.Asmcode = this.Asmcode + "8B13";
        }

        public void Mov_EDX_DWORD_Ptr_ECX()
        {
            this.Asmcode = this.Asmcode + "8B11";
        }

        public void Mov_EDX_DWORD_Ptr_EDX()
        {
            this.Asmcode = this.Asmcode + "8B12";
        }

        public void Mov_EDX_DWORD_Ptr_EDI()
        {
            this.Asmcode = this.Asmcode + "8B17";
        }

        public void Mov_EDX_DWORD_Ptr_ESI()
        {
            this.Asmcode = this.Asmcode + "8B16";
        }

        public void Mov_EDX_DWORD_Ptr_ESP()
        {
            this.Asmcode = this.Asmcode + "8B1424";
        }

        public void Mov_EAX_EBP()
        {
            this.Asmcode = this.Asmcode + "8BC5";
        }

        public void Mov_EAX_EBX()
        {
            this.Asmcode = this.Asmcode + "8BC3";
        }

        public void Mov_EAX_ECX()
        {
            this.Asmcode = this.Asmcode + "8BC1";
        }

        public void Mov_EAX_EDI()
        {
            this.Asmcode = this.Asmcode + "8BC7";
        }

        public void Mov_EAX_EDX()
        {
            this.Asmcode = this.Asmcode + "8BC2";
        }

        public void Mov_EAX_ESI()
        {
            this.Asmcode = this.Asmcode + "8BC6";
        }

        public void Mov_EAX_ESP()
        {
            this.Asmcode = this.Asmcode + "8BC4";
        }

        public void Mov_EBX_EBP()
        {
            this.Asmcode = this.Asmcode + "8BDD";
        }

        public void Mov_EBX_EAX()
        {
            this.Asmcode = this.Asmcode + "8BD8";
        }

        public void Mov_EBX_ECX()
        {
            this.Asmcode = this.Asmcode + "8BD9";
        }

        public void Mov_EBX_EDI()
        {
            this.Asmcode = this.Asmcode + "8BDF";
        }

        public void Mov_EBX_EDX()
        {
            this.Asmcode = this.Asmcode + "8BDA";
        }

        public void Mov_EBX_ESI()
        {
            this.Asmcode = this.Asmcode + "8BDE";
        }

        public void Mov_EBX_ESP()
        {
            this.Asmcode = this.Asmcode + "8BDC";
        }

        public void Mov_ECX_EBP()
        {
            this.Asmcode = this.Asmcode + "8BCD";
        }

       /* public void Mov_ECX_EAX()
        {
            this.Asmcode = this.Asmcode + "8BC8";
        }*/

        public void Mov_ECX_EBX()
        {
            this.Asmcode = this.Asmcode + "8BCB";
        }

        public void Mov_ECX_EDI()
        {
            this.Asmcode = this.Asmcode + "8BCF";
        }

        public void Mov_ECX_EDX()
        {
            this.Asmcode = this.Asmcode + "8BCA";
        }

        public void Mov_ECX_ESI()
        {
            this.Asmcode = this.Asmcode + "8BCE";
        }

        public void Mov_ECX_ESP()
        {
            this.Asmcode = this.Asmcode + "8BCC";
        }

        public void Mov_EDX_EBP()
        {
            this.Asmcode = this.Asmcode + "8BD5";
        }

        public void Mov_EDX_EBX()
        {
            this.Asmcode = this.Asmcode + "8BD3";
        }

        public void Mov_EDX_ECX()
        {
            this.Asmcode = this.Asmcode + "8BD1";
        }

        public void Mov_EDX_EDI()
        {
            this.Asmcode = this.Asmcode + "8BD7";
        }

        public void Mov_EDX_EAX()
        {
            this.Asmcode = this.Asmcode + "8BD0";
        }

        public void Mov_EDX_ESI()
        {
            this.Asmcode = this.Asmcode + "8BD6";
        }

        public void Mov_EDX_ESP()
        {
            this.Asmcode = this.Asmcode + "8BD4";
        }

        public void Mov_ESI_EBP()
        {
            this.Asmcode = this.Asmcode + "8BF5";
        }

        public void Mov_ESI_EBX()
        {
            this.Asmcode = this.Asmcode + "8BF3";
        }

        public void Mov_ESI_ECX()
        {
            this.Asmcode = this.Asmcode + "8BF1";
        }

        public void Mov_ESI_EDI()
        {
            this.Asmcode = this.Asmcode + "8BF7";
        }

        public void Mov_ESI_EAX()
        {
            this.Asmcode = this.Asmcode + "8BF0";
        }

        public void Mov_ESI_EDX()
        {
            this.Asmcode = this.Asmcode + "8BF2";
        }

        public void Mov_ESI_ESP()
        {
            this.Asmcode = this.Asmcode + "8BF4";
        }

        public void Mov_ESP_EBP()
        {
            this.Asmcode = this.Asmcode + "8BE5";
        }

        public void Mov_ESP_EBX()
        {
            this.Asmcode = this.Asmcode + "8BE3";
        }

        public void Mov_ESP_ECX()
        {
            this.Asmcode = this.Asmcode + "8BE1";
        }

        public void Mov_ESP_EDI()
        {
            this.Asmcode = this.Asmcode + "8BE7";
        }

        public void Mov_ESP_EAX()
        {
            this.Asmcode = this.Asmcode + "8BE0";
        }

        public void Mov_ESP_EDX()
        {
            this.Asmcode = this.Asmcode + "8BE2";
        }

        public void Mov_ESP_ESI()
        {
            this.Asmcode = this.Asmcode + "8BE6";
        }

        public void Mov_EDI_EBP()
        {
            this.Asmcode = this.Asmcode + "8BFD";
        }

        public void Mov_EDI_EAX()
        {
            this.Asmcode = this.Asmcode + "8BF8";
        }

        public void Mov_EDI_EBX()
        {
            this.Asmcode = this.Asmcode + "8BFB";
        }

        public void Mov_EDI_ECX()
        {
            this.Asmcode = this.Asmcode + "8BF9";
        }

        public void Mov_EDI_EDX()
        {
            this.Asmcode = this.Asmcode + "8BFA";
        }

        public void Mov_EDI_ESI()
        {
            this.Asmcode = this.Asmcode + "8BFE";
        }

        public void Mov_EDI_ESP()
        {
            this.Asmcode = this.Asmcode + "8BFC";
        }

        public void Mov_EBP_EDI()
        {
            this.Asmcode = this.Asmcode + "8BDF";
        }

        public void Mov_EBP_EAX()
        {
            this.Asmcode = this.Asmcode + "8BE8";
        }

        public void Mov_EBP_EBX()
        {
            this.Asmcode = this.Asmcode + "8BEB";
        }

        public void Mov_EBP_ECX()
        {
            this.Asmcode = this.Asmcode + "8BE9";
        }

        public void Mov_EBP_EDX()
        {
            this.Asmcode = this.Asmcode + "8BEA";
        }

        public void Mov_EBP_ESI()
        {
            this.Asmcode = this.Asmcode + "8BEE";
        }

        public void Mov_EBP_ESP()
        {
            this.Asmcode = this.Asmcode + "8BEC";
        }
        #endregion

        #region Push
        public void Push(int addr)
        {
                if ((addr <= 127) && (addr >= -128))
                    this.Asmcode = this.Asmcode + "6A" + intTohex(addr, 2);
                else
                    this.Asmcode = this.Asmcode + "68" + intTohex(addr, 8);
        }

        public void Push_DWORD_Ptr(int addr)
        {
            this.Asmcode = this.Asmcode + "FF35" + intTohex(addr, 8);
        }

        public void Push_EAX()
        {
                this.Asmcode = this.Asmcode + "50";
        }

        public void Push_ECX()
        {

                this.Asmcode = this.Asmcode + "51";
        }

        public void Push_EDX()
        {
                this.Asmcode = this.Asmcode + "52";
        }

        public void Push_EBX()
        {
                this.Asmcode = this.Asmcode + "53";
        }

        public void Push_ESP()
        {
                this.Asmcode = this.Asmcode + "54";
        }

        public void Push_EBP()
        {
                this.Asmcode = this.Asmcode + "55";
        }

        public void Push_ESI()
        {
                this.Asmcode = this.Asmcode + "56";
        }

        public void Push_EDI()
        {
                this.Asmcode = this.Asmcode + "57";
        }
        #endregion

        #region Call
        public void Call_EAX()
        {
            this.Asmcode = this.Asmcode + "FFD0";
        }

        public void Call_EBX()
        {
            this.Asmcode = this.Asmcode + "FFD3";
        }

        public void Call_ECX()
        {
            this.Asmcode = this.Asmcode + "FFD1";
        }

        public void Call_EDX()
        {
            this.Asmcode = this.Asmcode + "FFD2";
        }

        public void Call_ESI()
        {
            this.Asmcode = this.Asmcode + "FFD2";
        }

        public void Call_ESP()
        {
            this.Asmcode = this.Asmcode + "FFD4";
        }

        public void Call_EBP()
        {
            this.Asmcode = this.Asmcode + "FFD5";
        }

        public void Call_EDI()
        {
            this.Asmcode = this.Asmcode + "FFD7";
        }

        public void Call_DWORD_Ptr(int addr)
        {
            this.Asmcode = this.Asmcode + "FF15" + intTohex(addr, 8);
        }

        public void Call_DWORD_Ptr_EAX()
        {
            this.Asmcode = this.Asmcode + "FF10";
        }

        public void Call_DWORD_Ptr_EBX()
        {
            this.Asmcode = this.Asmcode + "FF13";
        }
        #endregion

        #region Lea
        public void Lea_EAX_DWORD_Ptr_EAX_Add(int addr)
        {
            if ((addr <= 127) && (addr >= -128))
                this.Asmcode = this.Asmcode + "8D40" + intTohex(addr, 2);
            else
                this.Asmcode = this.Asmcode + "8D80" + intTohex(addr, 8);
        }

        public void Lea_EAX_DWORD_Ptr_EBX_Add(int addr)
        {
            if ((addr <= 127) && (addr >= -128))
                this.Asmcode = this.Asmcode + "8D43" + intTohex(addr, 2);
            else
                this.Asmcode = this.Asmcode + "8D83" + intTohex(addr, 8);
        }

        public void Lea_EAX_DWORD_Ptr_ECX_Add(int addr)
        {
            if ((addr <= 127) && (addr >= -128))
                this.Asmcode = this.Asmcode + "8D41" + intTohex(addr, 2);
            else
                this.Asmcode = this.Asmcode + "8D81" + intTohex(addr, 8);
        }

        public void Lea_EAX_DWORD_Ptr_EDX_Add(int addr)
        {
            if ((addr <= 127) && (addr >= -128))
                this.Asmcode = this.Asmcode + "8D42" + intTohex(addr, 2);
            else
                this.Asmcode = this.Asmcode + "8D82" + intTohex(addr, 8);
        }

        public void Lea_EAX_DWORD_Ptr_ESI_Add(int addr)
        {
            if ((addr <= 127) && (addr >= -128))
                this.Asmcode = this.Asmcode + "8D46" + intTohex(addr, 2);
            else
                this.Asmcode = this.Asmcode + "8D86" + intTohex(addr, 8);
        }

        public void Lea_EAX_DWORD_Ptr_ESP_Add(int addr)
        {
            if ((addr <= 127) && (addr >= -128))
                this.Asmcode = this.Asmcode + "8D40" + intTohex(addr, 2);
            else
                this.Asmcode = this.Asmcode + "8D80" + intTohex(addr, 8);
        }

        public void Lea_EAX_DWORD_Ptr_EBP_Add(int addr)
        {
            if ((addr <= 127) && (addr >= -128))
                this.Asmcode = this.Asmcode + "8D4424" + intTohex(addr, 2);
            else
                this.Asmcode = this.Asmcode + "8D8424" + intTohex(addr, 8);
        }

        public void Lea_EAX_DWORD_Ptr_EDI_Add(int addr)
        {
            if ((addr <= 127) && (addr >= -128))
                this.Asmcode = this.Asmcode + "8D47" + intTohex(addr, 2);
            else
                this.Asmcode = this.Asmcode + "8D87" + intTohex(addr, 8);
        }

        public void Lea_EBX_DWORD_Ptr_EAX_Add(int addr)
        {
            if ((addr <= 127) && (addr >= -128))
                this.Asmcode = this.Asmcode + "8D58" + intTohex(addr, 2);
            else
                this.Asmcode = this.Asmcode + "8D98" + intTohex(addr, 8);
        }

        public void Lea_EBX_DWORD_Ptr_ESP_Add(int addr)
        {
            if ((addr <= 127) && (addr >= -128))
                this.Asmcode = this.Asmcode + "8D5C24" + intTohex(addr, 2);
            else
                this.Asmcode = this.Asmcode + "8D9C24" + intTohex(addr, 8);
        }

        public void Lea_EBX_DWORD_Ptr_EBX_Add(int addr)
        {
            if ((addr <= 127) && (addr >= -128))
                this.Asmcode = this.Asmcode + "8D5B" + intTohex(addr, 2);
            else
                this.Asmcode = this.Asmcode + "8D9B" + intTohex(addr, 8);
        }

        public void Lea_EBX_DWORD_Ptr_ECX_Add(int addr)
        {
            if ((addr <= 127) && (addr >= -128))
                this.Asmcode = this.Asmcode + "8D59" + intTohex(addr, 2);
            else
                this.Asmcode = this.Asmcode + "8D99" + intTohex(addr, 8);
        }

        public void Lea_EBX_DWORD_Ptr_EDX_Add(int addr)
        {
            if ((addr <= 127) && (addr >= -128))
                this.Asmcode = this.Asmcode + "8D5A" + intTohex(addr, 2);
            else
                this.Asmcode = this.Asmcode + "8D9A" + intTohex(addr, 8);
        }

        public void Lea_EBX_DWORD_Ptr_EDI_Add(int addr)
        {
            if ((addr <= 127) && (addr >= -128))
                this.Asmcode = this.Asmcode + "8D5F" + intTohex(addr, 2);
            else
                this.Asmcode = this.Asmcode + "8D9F" + intTohex(addr, 8);
        }

        public void Lea_EBX_DWORD_Ptr_EBP_Add(int addr)
        {
            if((addr<=127)&&(addr>=-128))
                this.Asmcode = this.Asmcode + "8D5D" + intTohex(addr, 2);
            else
                this.Asmcode = this.Asmcode + "8D9D" + intTohex(addr, 8);
        }

        public void Lea_EBX_DWORD_Ptr_ESI_Add(int addr)
        {
            if ((addr <= 127) && (addr >= -128))
                this.Asmcode = this.Asmcode + "8D5E" + intTohex(addr, 2);
            else
                this.Asmcode = this.Asmcode + "8D9E" + intTohex(addr, 8);
        }

        public void Lea_ECX_DWORD_Ptr_EAX_Add(int addr)
        {
            if ((addr <= 127) && (addr >= -128))
                this.Asmcode = this.Asmcode + "8D48" + intTohex(addr, 2);
            else
                this.Asmcode = this.Asmcode + "8D88" + intTohex(addr, 8);
        }

        public void Lea_ECX_DWORD_Ptr_ESP_Add(int addr)
        {
            if ((addr <= 127) && (addr >= -128))
                this.Asmcode = this.Asmcode + "8D4C24" + intTohex(addr, 2);
            else
                this.Asmcode = this.Asmcode + "8D8C24" + intTohex(addr, 8);
        }

        public void Lea_ECX_DWORD_Ptr_EBX_Add(int addr)
        {
            if ((addr <= 127) && (addr >= -128))
                this.Asmcode = this.Asmcode + "8D4B" + intTohex(addr, 2);
            else
                this.Asmcode = this.Asmcode + "8D8B" + intTohex(addr, 8);
        }

        public void Lea_ECX_DWORD_Ptr_ECX_Add(int addr)
        {
            if ((addr <= 127) && (addr >= -128))
                this.Asmcode = this.Asmcode + "8D49" + intTohex(addr, 2);
            else
                this.Asmcode = this.Asmcode + "8D89" + intTohex(addr, 8);
        }

        public void Lea_ECX_DWORD_Ptr_EDX_Add(int addr)
        {
            if ((addr <= 127) && (addr >= -128))
                this.Asmcode = this.Asmcode + "8D4A" + intTohex(addr, 2);
            else
                this.Asmcode = this.Asmcode + "8D8A" + intTohex(addr, 8);
        }

        public void Lea_ECX_DWORD_Ptr_EDI_Add(int addr)
        {
            if ((addr <= 127) && (addr >= -128))
                this.Asmcode = this.Asmcode + "8D4F" + intTohex(addr, 2);
            else
                this.Asmcode = this.Asmcode + "8D8F" + intTohex(addr, 8);
        }

        public void Lea_ECX_DWORD_Ptr_EBP_Add(int addr)
        {
            if ((addr <= 127) && (addr >= -128))
                this.Asmcode = this.Asmcode + "8D4D" + intTohex(addr, 2);
            else
                this.Asmcode = this.Asmcode + "8D8D" + intTohex(addr, 8);
        }

        public void Lea_ECX_DWORD_Ptr_ESI_Add(int addr)
        {
            if ((addr <= 127) && (addr >= -128))
                this.Asmcode = this.Asmcode + "8D4E" + intTohex(addr, 2);
            else
                this.Asmcode = this.Asmcode + "8D8E" + intTohex(addr, 8);
        }

        public void Lea_EDX_DWORD_Ptr_EAX_Add(int addr)
        {
            if ((addr <= 127) && (addr >= -128))
                this.Asmcode = this.Asmcode + "8D50" + intTohex(addr, 2);
            else
                this.Asmcode = this.Asmcode + "8D90" + intTohex(addr, 8);
        }

        public void Lea_EDX_DWORD_Ptr_ESP_Add(int addr)
        {
            if ((addr <= 127) && (addr >= -128))
                this.Asmcode = this.Asmcode + "8D5424" + intTohex(addr, 2);
            else
                this.Asmcode = this.Asmcode + "8D9424" + intTohex(addr, 8);
        }

        public void Lea_EDX_DWORD_Ptr_EBX_Add(int addr)
        {
            if ((addr <= 127) && (addr >= -128))
                this.Asmcode = this.Asmcode + "8D53" + intTohex(addr, 2);
            else
                this.Asmcode = this.Asmcode + "8D93" + intTohex(addr, 8);
        }

        public void Lea_EDX_DWORD_Ptr_ECX_Add(int addr)
        {
            if ((addr <= 127) && (addr >= -128))
                this.Asmcode = this.Asmcode + "8D51" + intTohex(addr, 2);
            else
                this.Asmcode = this.Asmcode + "8D91" + intTohex(addr, 8);
        }

        public void Lea_EDX_DWORD_Ptr_EDX_Add(int addr)
        {
            if ((addr <= 127) && (addr >= -128))
                this.Asmcode = this.Asmcode + "8D52" + intTohex(addr, 2);
            else
                this.Asmcode = this.Asmcode + "8D92" + intTohex(addr, 8);
        }

        public void Lea_EDX_DWORD_Ptr_EDI_Add(int addr)
        {
            if ((addr <= 127) && (addr >= -128))
                this.Asmcode = this.Asmcode + "8D57" + intTohex(addr, 2);
            else
                this.Asmcode = this.Asmcode + "8D97" + intTohex(addr, 8);
        }

        public void Lea_EDX_DWORD_Ptr_EBP_Add(int addr)
        {
            if ((addr <= 127) && (addr >= -128))
                this.Asmcode = this.Asmcode + "8D55" + intTohex(addr, 2);
            else
                this.Asmcode = this.Asmcode + "8D95" + intTohex(addr, 8);
        }

        public void Lea_EDX_DWORD_Ptr_ESI_Add(int addr)
        {
            if ((addr <= 127) && (addr >= -128))
                this.Asmcode = this.Asmcode + "8D56" + intTohex(addr, 2);
            else
                this.Asmcode = this.Asmcode + "8D96" + intTohex(addr, 8);
        }
        #endregion

        #region POP
        public void Pop_EAX()
        {
            this.Asmcode = this.Asmcode + "58";
        }

        public void Pop_EBX()
        {
            this.Asmcode = this.Asmcode + "5B";
        }

        public void Pop_ECX()
        {
            this.Asmcode = this.Asmcode + "59";
        }

        public void Pop_EDX()
        {
            this.Asmcode = this.Asmcode + "5A";
        }

        public void Pop_ESI()
        {
            this.Asmcode = this.Asmcode + "5E";
        }

        public void Pop_ESP()
        {
            this.Asmcode = this.Asmcode + "5C";
        }

        public void Pop_EDI()
        {
            this.Asmcode = this.Asmcode + "5F";
        }

        public void Pop_EBP()
        {
            this.Asmcode = this.Asmcode + "5D";
        }
        #endregion


        #region CMP
        public void Cmp_EAX(int addr)
        {
            if ((addr <= 127) && (addr >= -128))
                this.Asmcode = this.Asmcode + "83F8" + intTohex(addr, 2);
            else
                this.Asmcode = this.Asmcode + "3D" + intTohex(addr, 8);
        }

        public void Cmp_EAX_EDX()
        {
            this.Asmcode = this.Asmcode + "3BC2";
        }

        public void Cmp_EAX_DWORD_Ptr(int addr)
        {
            this.Asmcode = this.Asmcode + "3B05" + intTohex(addr, 8);
        }

        public void Cmp_DWORD_Ptr_EAX(int addr)
        {
            this.Asmcode = this.Asmcode + "3905" + intTohex(addr, 8);
        }
        #endregion

        #region DEC
        public void Dec_EAX()
        {
            this.Asmcode = this.Asmcode + "48";
        }

        public void Dec_EBX()
        {
            this.Asmcode = this.Asmcode + "4B";
        }

        public void Dec_ECX()
        {
            this.Asmcode = this.Asmcode + "49";
        }

        public void Dec_EDX()
        {
            this.Asmcode = this.Asmcode + "4A";
        }
        #endregion

        #region idiv
        public void Idiv_EAX()
        {
            this.Asmcode = this.Asmcode + "F7F8";
        }

        public void Idiv_EBX()
        {
            this.Asmcode = this.Asmcode + "F7FB";
        }

        public void Idiv_ECX()
        {
            this.Asmcode = this.Asmcode + "F7F9";
        }

        public void Idiv_EDX()
        {
            this.Asmcode = this.Asmcode + "F7FA";
        }
        #endregion

        #region Imul
        public void Imul_EAX_EDX()
        {
            this.Asmcode = this.Asmcode + "0FAFC2";
        }

        public void Imul_EAX(int addr)
        {
            this.Asmcode = this.Asmcode + "6BC0" + intTohex(addr, 2);
        }

        public void ImulB_EAX(int addr)
        {
            this.Asmcode = this.Asmcode + "69C0" + intTohex(addr, 8);
        }
        #endregion

        #region Inc
        public void Inc_EAX()
        {
            this.Asmcode = this.Asmcode + "40";
        }

        public void Inc_EBX()
        {
            this.Asmcode = this.Asmcode + "43";
        }

        public void Inc_ECX()
        {
            this.Asmcode = this.Asmcode + "41";
        }

        public void Inc_EDX()
        {
            this.Asmcode = this.Asmcode + "42";
        }

        public void Inc_EDI()
        {
            this.Asmcode = this.Asmcode + "47";
        }

        public void Inc_ESI()
        {
            this.Asmcode = this.Asmcode + "46";
        }

        public void Inc_DWORD_Ptr_EAX()
        {
            this.Asmcode = this.Asmcode + "FF00";
        }

        public void Inc_DWORD_Ptr_EBX()
        {
            this.Asmcode = this.Asmcode + "FF03";
        }

        public void Inc_DWORD_Ptr_ECX()
        {
            this.Asmcode = this.Asmcode + "FF01";
        }

        public void Inc_DWORD_Ptr_EDX()
        {
            this.Asmcode = this.Asmcode + "FF02";
        }
        #endregion

        #region jmp
        public void JMP_EAX()
        {
            this.Asmcode = this.Asmcode + "FFE0";
        }
        #endregion
    }
}
