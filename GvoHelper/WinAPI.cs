using System;
using System.Collections.Generic;

using System.Text;
using System.Runtime.InteropServices;
using System.IO;

namespace GvoHelper
{
    class WinAPI
    {
        //取得電腦tick時間
        [DllImport("winmm", EntryPoint = "timeGetTime")]
        public static extern int timeGetTime();

        [DllImport("user32.dll", EntryPoint = "GetClassName")]
        public static extern int GetClassName(
            IntPtr hWnd,
            StringBuilder lpClassName,
            int nMaxCount);

        //尋找視窗
        [DllImport("user32", EntryPoint = "FindWindow")]
        public static extern IntPtr FindWindow(
            string lpClassName,
            string lpWindowName);
        //改視窗title
        [DllImport("user32", EntryPoint = "SetWindowText")]
        public static extern int SetWindowText(
            IntPtr hWnd,
            string lpString);
        //視窗置前
        [DllImport("user32", EntryPoint = "SetForegroundWindow")]
        public static extern void SetForegroundWindow(
            IntPtr hWnd);
        //取得ProcessID
        [DllImport("user32", EntryPoint = "GetWindowThreadProcessId")]
        public static extern int GetWindowThreadProcessId(
            IntPtr hWnd,
            ref int lpdwProcessId);
        //開啟Process
        [DllImport("kernel32", EntryPoint = "OpenProcess")]
        public static extern int OpenProcess(
            int dwDesiredAccess,
            int bInheritHandle,
            int dwProcessId);

        [DllImport("kernel32", EntryPoint = "CloseHandle")]
        public static extern int CloseHandle(int hObject);


        public const int OPEN_PROCESS_ALL = 0x1F0FFF;
        public const int PROCESS_CREATE_THREAD = 0x2;
        public const int PROCESS_VM_OPERATION = 0x8;
        public const int PROCESS_VM_READ = 0x10;
        public const int PROCESS_VM_WRITE = 0x20;

        //讀取記憶體
        [DllImport("kernel32", EntryPoint = "ReadProcessMemory")]
        public static extern int ReadProcessMemory(int hProcess, int lpBaseAddress, byte lpBuffer, int nSize, int lpNumberOfBytesWritten);
        [DllImport("kernel32", EntryPoint = "ReadProcessMemory")]
        public static extern int ReadProcessMemory(int hProcess, int lpBaseAddress, byte[] lpBuffer, int nSize, ref int lpNumberOfBytesWritten);
        [DllImport("kernel32", EntryPoint = "ReadProcessMemory")]
        public static extern int ReadProcessMemory(IntPtr hProcess, ref int lpBaseAddress, ref int lpBuffer, int nSize, int lpNumberOfBytesWritten);
        [DllImport("kernel32", EntryPoint = "ReadProcessMemory")]
        public static extern int ReadProcessMemory(int hProcess, int lpBaseAddress, char lpBuffer, int nSize, int lpNumberOfBytesWritten);
        [DllImport("kernel32", EntryPoint = "ReadProcessMemory")]
        public static extern int ReadProcessMemory(int hProcess, int lpBaseAddress, out string lpBuffer, int nSize, int lpNumberOfBytesWritten);
        [DllImport("kernel32", EntryPoint = "ReadProcessMemory")]
        public static extern int ReadProcessMemory(int hProcess, int lpBaseAddress, byte[] lpBuffer, uint nSize, int lpNumberOfBytesWritten);
        [DllImport("kernel32", EntryPoint = "ReadProcessMemory")]
        public static extern int ReadProcessMemory(int hProcess, int lpBaseAddress, out double lpBuffer, int nSize, int lpNumberOfBytesWritten);
        [DllImport("kernel32", EntryPoint = "ReadProcessMemory")]
        public static extern int ReadProcessMemory(int hProcess, int lpBaseAddress, out byte lpBuffer, int nSize, int lpNumberOfBytesWritten);
        [DllImport("kernel32", EntryPoint = "ReadProcessMemory")]
        public static extern int ReadProcessMemory(int hProcess, int lpBaseAddress, out int lpBuffer, int nSize, int lpNumberOfBytesWritten);
        [DllImport("kernel32", EntryPoint = "ReadProcessMemory")]
        public static extern int ReadProcessMemory(IntPtr hProcess, int lpBaseAddress, ref int lpBuffer, int nSize, int lpNumberOfBytesWritten);
        [DllImport("kernel32", EntryPoint = "ReadProcessMemory")]
        public static extern int ReadProcessMemory(int hProcess, int lpBaseAddress, out float lpBuffer, int nSize, int lpNumberOfBytesWritten);
        [DllImport("kernel32", EntryPoint = "ReadProcessMemory")]
        public static extern int ReadProcessMemory(int hProcess, int lpBaseAddress, out char lpBuffer, int nSize, int lpNumberOfBytesWritten);
        [DllImport("kernel32", EntryPoint = "ReadProcessMemory")]
        public static extern int ReadProcessMemory(int hProcess, int lpBaseAddress, ref int lpBuffer, int nSize, ref int lpNumberOfBytesWritten);
        [DllImport("kernel32", EntryPoint = "ReadProcessMemory")]
        public static extern int ReadProcessMemory(int hProcess, IntPtr lpBaseAddress, ref int lpBuffer, int nSize, ref int lpNumberOfBytesWritten);
        [DllImport("kernel32", EntryPoint = "ReadProcessMemory")]
        public static extern int ReadProcessMemory(int hProcess, out IntPtr lpBaseAddress, out int lpBuffer, int nSize, int lpNumberOfBytesWritten);
        [DllImport("kernel32", EntryPoint = "ReadProcessMemory")]
        public static extern int ReadProcessMemory(int hProcess, IntPtr lpBaseAddress, out int lpBuffer, int nSize, int lpNumberOfBytesWritten);
        [DllImport("kernel32", EntryPoint = "ReadProcessMemory")]
        public static extern int ReadProcessMemory(int hProcess, IntPtr lpBaseAddress, int lpBuffer, int nSize, int lpNumberOfBytesWritten);
        [DllImport("kernel32", EntryPoint = "ReadProcessMemory")]
        public static extern int ReadProcessMemory(int hProcess, ref IntPtr lpBaseAddress, ref int lpBuffer, int nSize, ref int lpNumberOfBytesWritten);
        [DllImport("kernel32", EntryPoint = "ReadProcessMemory")]
        public static extern int ReadProcessMemory(int hProcess, ref int lpBaseAddress, ref int lpBuffer, int nSize, ref int lpNumberOfBytesWritten);
        [DllImport("kernel32", EntryPoint = "ReadProcessMemory")] 
        public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [Out] byte[] lpBuffer, int dwSize, out int lpNumberOfBytesRead);
        //寫入記憶體
        [DllImport("kernel32", EntryPoint = "WriteProcessMemory")]
        public static extern int WriteProcessMemory(int hProcess, int lpBaseAddress, byte[] buffer, int size, int lpNumberOfBytesWritten);
        
        [DllImport("kernel32", EntryPoint = "WriteProcessMemory")]
        public static extern int WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] buffer, int size, out IntPtr lpNumberOfBytesWritten);
        [DllImport("kernel32", EntryPoint = "WriteProcessMemory")]
        public static extern int WriteProcessMemory(int hProcess, ref int lpBaseAddress, ref char[] lpBuffer, int nSize, ref int lpNumberOfBytesWritten);
        [DllImport("kernel32", EntryPoint = "WriteProcessMemory")]
        public static extern int WriteProcessMemory(int hProcess, ref int lpBaseAddress, ref int lpBuffer, int nSize, ref int lpNumberOfBytesWritten);
        //送出訊息，按鍵
        [DllImport("user32", EntryPoint = "PostMessage")]
        public static extern IntPtr PostMessage(IntPtr hWnd, int wMsg, IntPtr wParam, UIntPtr lParam);

        public const int WM_KEYDOWN = 0x0100;
        public const int WM_KEYUP = 0x0101;
        public const int WM_CHAR = 0x0102;
        public const int VK_TAB = 0x09;
        public const int VK_ENTER = 0x0D;
        public const int VK_UP = 0x26;
        public const int VK_DOWN = 0x28;
        public const int VK_RIGHT = 0x27; 


        //取得虛擬碼
        [DllImport("user32", EntryPoint = "MapVirtualKey")]
        public static extern uint MapVirtualKey(uint uCode, uint uMapType);

        //設定視窗相關
        [DllImport("user32", EntryPoint = "ShowWindow")]
        public static extern int ShowWindow(IntPtr hWnd, int nCmdShow);

        public enum CommandShow : int
        {
            SW_HIDE = 0,
            SW_SHOWNORMAL = 1,
            SW_NORMAL = 1,
            SW_SHOWMINIMIZED = 2,
            SW_SHOWMAXIMIZED = 3,
            SW_MAXIMIZE = 3,
            SW_SHOWNOACTIVATE = 4,
            SW_SHOW = 5,
            SW_MINIMIZE = 6,
            SW_SHOWMINNOACTIVE = 7,
            SW_SHOWNA = 8,
            SW_RESTORE = 9,
            SW_SHOWDEFAULT = 10,
            SW_FORCEMINIMIZE = 11,
        }

        [DllImport("Kernel32.dll", CharSet = CharSet.Ansi)]
        public static extern bool CreateProcess(
            StringBuilder lpApplicationName,
            StringBuilder lpCommandLine,
            SECURITY_ATTRIBUTES lpProcessAttributes,
            SECURITY_ATTRIBUTES lpThreadAttributes,
            bool bInheritHandles,
            ProcessCreationFlags dwCreationFlags,
            StringBuilder lpEnvironment,
            StringBuilder lpCurrentDirectory,
            ref STARTUPINFO lpStartupInfo,
            ref PROCESS_INFORMATION lpProcessInformation
            );

        public enum ProcessCreationFlags : uint
        {
            ZERO_FLAG = 0x00000000,
            CREATE_BREAKAWAY_FROM_JOB = 0x01000000,
            CREATE_DEFAULT_ERROR_MODE = 0x04000000,
            CREATE_NEW_CONSOLE = 0x00000010,
            CREATE_NEW_PROCESS_GROUP = 0x00000200,
            CREATE_NO_WINDOW = 0x08000000,
            CREATE_PROTECTED_PROCESS = 0x00040000,
            CREATE_PRESERVE_CODE_AUTHZ_LEVEL = 0x02000000,
            CREATE_SEPARATE_WOW_VDM = 0x00001000,
            CREATE_SHARED_WOW_VDM = 0x00001000,
            CREATE_SUSPENDED = 0x00000004,
            CREATE_UNICODE_ENVIRONMENT = 0x00000400,
            DEBUG_ONLY_THIS_PROCESS = 0x00000002,
            DEBUG_PROCESS = 0x00000001,
            DETACHED_PROCESS = 0x00000008,
            EXTENDED_STARTUPINFO_PRESENT = 0x00080000,
            INHERIT_PARENT_AFFINITY = 0x00010000
        }

        public enum ThreadAccess : int
        {
            TERMINATE = (0x0001),
            SUSPEND_RESUME = (0x0002),
            GET_CONTEXT = (0x0008),
            SET_CONTEXT = (0x0010),
            SET_INFORMATION = (0x0020),
            QUERY_INFORMATION = (0x0040),
            SET_THREAD_TOKEN = (0x0080),
            IMPERSONATE = (0x0100),
            DIRECT_IMPERSONATION = (0x0200)
        }

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenThread(ThreadAccess dwDesiredAccess, bool bInheritHandle, uint dwThreadId);

        [DllImport("kernel32.dll")]
        public static extern uint SuspendThread(IntPtr hThread);

        [DllImport("kernel32.dll")]
        public static extern int ResumeThread(IntPtr hThread);


        [StructLayout(LayoutKind.Sequential)]
        public class SECURITY_ATTRIBUTES
        {
            public int nLength;
            public string lpSecurityDescriptor;
            public bool bInheritHandle;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct STARTUPINFO
        {
            public int cb;
            public string lpReserved;
            public string lpDesktop;
            public int lpTitle;
            public int dwX;
            public int dwY;
            public int dwXSize;
            public int dwYSize;
            public int dwXCountChars;
            public int dwYCountChars;
            public int dwFillAttribute;
            public int dwFlags;
            public int wShowWindow;
            public int cbReserved2;
            public byte lpReserved2;
            public IntPtr hStdInput;
            public IntPtr hStdOutput;
            public IntPtr hStdError;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct PROCESS_INFORMATION
        {
            public IntPtr hProcess;
            public IntPtr hThread;
            public int dwProcessId;
            public int dwThreadId;
        }

        #region Win32 Api : WaitForSingleObject
        //检测一个系统核心对象(线程，事件，信号)的信号状态，当对象执行时间超过dwMilliseconds就返回，否则就一直等待对象返回信号
        [DllImport("Kernel32.dll")]
        public static extern uint WaitForSingleObject(IntPtr hHandle, uint dwMilliseconds);

        #endregion

        #region Win32 Api : CloseHandle
        //关闭一个内核对象,释放对象占有的系统资源。其中包括文件、文件映射、进程、线程、安全和同步对象等
        [DllImport("Kernel32.dll")]
        public static extern bool CloseHandle(IntPtr hObject);
        #endregion

        #region Win32 Api : GetExitCodeProcess
        //获取一个已中断进程的退出代码,非零表示成功，零表示失败。
        //参数hProcess，想获取退出代码的一个进程的句柄，参数lpExitCode，用于装载进程退出代码的一个长整数变量。
        [DllImport("Kernel32.dll")]
        static extern bool GetExitCodeProcess(IntPtr hProcess, ref uint lpExitCode);

        #endregion

        public class SearchData
        {
            public string Wndclass;
            public string Title;
            public IntPtr hWnd;
        }

        [DllImport("user32", EntryPoint = "EnumWindows")]
        public static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, ref SearchData data);

        public delegate bool EnumWindowsProc(IntPtr hWnd, ref SearchData data);

        [DllImport("kernel32.dll")]
        public static extern long VirtualQueryEx(IntPtr hProcess, IntPtr lpAddress, out MEMORY_INF lpBuffer, long dwLength);

        [DllImport("kernel32.dll")]
        public static extern bool VirtualQueryEx(IntPtr hProcess, IntPtr lpAddress, out MEMORY_BASIC_INFORMATION lpBuffer, uint dwLength);

        [StructLayout(LayoutKind.Sequential)]
        public struct MEMORY_BASIC_INFORMATION
        {
            public IntPtr BaseAddress;
            public IntPtr AllocationBase;
            public uint AllocationProtect;
            public int RegionSize;
            public MEMMessage State;
            public MEMMessage Protect;
            public int Type;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MEMORY_INF
        {
            public int BaseAddress;
            public int AllocationBase;
            public int AllocationProtect;
            public int RegionSize;
            public MEMMessage State;
            public MEMMessage Protect;
            public int lType;
        }

        public enum MEMMessage
        {
            PAGE_NOACCESS = 0x01,
            PAGE_READONLY = 0x02,
            PAGE_READWRITE = 0x04,
            PAGE_WRITECOPY = 0x08,
            PAGE_EXECUTE = 0x10,
            PAGE_EXECUTE_READ = 0x20,
            PAGE_EXECUTE_READWRITE = 0x40,
            PAGE_EXECUTE_WRITECOPY = 0x80,
            PAGE_GUARD = 0x100,
            PAGE_NOCACHE = 0x200,
            PAGE_WRITECOMBINE = 0x400,
            MEM_COMMIT = 0x1000,
            MEM_RESERVE = 0x2000,
            MEM_DECOMMIT = 0x4000,
            MEM_RELEASE = 0x8000,
            MEM_FREE = 0x10000,
            MEM_PRIVATE = 0x20000,
            MEM_MAPPED = 0x40000,
            MEM_RESET = 0x80000,
            MEM_TOP_DOWN = 0x100000,
            MEM_WRITE_WATCH = 0x200000,
            MEM_PHYSICAL = 0x400000,
            MEM_ROTATE = 0x800000,
            MEM_LARGE_PAGES = 0x20000000,
        }
        
        /////////////////////////
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetCurrentThreadId();
        [DllImport("user32.dll")]
        public static extern IntPtr SetWindowsHookEx(int idHook, HOOKPROC lpfn, IntPtr hInstance, IntPtr threadId);
        [DllImport("user32.dll")]
        public static extern bool UnhookWindowsHookEx(IntPtr hHook);
        [DllImport("user32.dll")]
        public static extern IntPtr CallNextHookEx(IntPtr hHook, int nCode, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll")]
        public static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        public delegate IntPtr HOOKPROC(int nCode, IntPtr wParam, IntPtr lParam);

        public const int GWL_HINSTANCE = (-6);
        public const int WH_CBT = 5;
        public const int HCBT_ACTIVATE = 5;

        public const int SWP_NOSIZE = 0x0001;
        public const int SWP_NOZORDER = 0x0004;
        public const int SWP_NOACTIVATE = 0x0010;

        public struct RECT
        {
            public RECT(int left, int top, int right, int bottom)
            {
                Left = left;
                Top = top;
                Right = right;
                Bottom = bottom;
            }

            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }
    }
}