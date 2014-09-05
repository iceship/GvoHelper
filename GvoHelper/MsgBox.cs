using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GvoHelper
{
    public class MsgBox
    {
        private IWin32Window m_ownerWindow = null;
        private IntPtr m_hHook = (IntPtr)0;

        public static DialogResult Show(
            IWin32Window owner,
            string messageBoxText,
            string caption,
            MessageBoxButtons button,
            MessageBoxIcon icon)
        {
            MsgBox mbox = new MsgBox(owner);
            return mbox.Show(messageBoxText, caption, button, icon);
        }

        private MsgBox(IWin32Window window)
        {
            m_ownerWindow = window;
        }

        private DialogResult Show(
            string messageBoxText,
            string caption,
            MessageBoxButtons button,
            MessageBoxIcon icon)
        {
            IntPtr hInstance = WinAPI.GetWindowLong(m_ownerWindow.Handle, WinAPI.GWL_HINSTANCE);
            IntPtr threadId = WinAPI.GetCurrentThreadId();
            m_hHook = WinAPI.SetWindowsHookEx(WinAPI.WH_CBT, new WinAPI.HOOKPROC(HookProc), hInstance, threadId);

            return MessageBox.Show(m_ownerWindow, messageBoxText, caption, button, icon);
        }

        private IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode == WinAPI.HCBT_ACTIVATE)
            {
                WinAPI.RECT rcForm = new WinAPI.RECT(0, 0, 0, 0);
                WinAPI.RECT rcMsgBox = new WinAPI.RECT(0, 0, 0, 0);

                WinAPI.GetWindowRect(m_ownerWindow.Handle, out rcForm);
                WinAPI.GetWindowRect(wParam, out rcMsgBox);

                int x = (rcForm.Left + (rcForm.Right - rcForm.Left) / 2) - ((rcMsgBox.Right - rcMsgBox.Left) / 2);
                int y = (rcForm.Top + (rcForm.Bottom - rcForm.Top) / 2) - ((rcMsgBox.Bottom - rcMsgBox.Top) / 2);

                WinAPI.SetWindowPos(wParam, 0, x, y, 0, 0, WinAPI.SWP_NOSIZE | WinAPI.SWP_NOZORDER | WinAPI.SWP_NOACTIVATE);

                IntPtr result = WinAPI.CallNextHookEx(m_hHook, nCode, wParam, lParam);

                WinAPI.UnhookWindowsHookEx(m_hHook);
                m_hHook = (IntPtr)0;

                return result;
            }
            else
                return WinAPI.CallNextHookEx(m_hHook, nCode, wParam, lParam);
        }
    }
}
