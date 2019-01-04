using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;

namespace PEDScannerLib
{
    public class SmartSuggestionEngine
    {

        #region definitions
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr LocalFree(IntPtr hMem);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern int FormatMessage(FormatMessageFlags dwFlags, IntPtr lpSource, uint dwMessageId, uint dwLanguageId, ref IntPtr lpBuffer, uint nSize, IntPtr Arguments);

        [Flags]
        private enum FormatMessageFlags : uint
        {
            FORMAT_MESSAGE_ALLOCATE_BUFFER = 0x00000100,
            FORMAT_MESSAGE_IGNORE_INSERTS = 0x00000200,
            FORMAT_MESSAGE_FROM_SYSTEM = 0x00001000,
            FORMAT_MESSAGE_ARGUMENT_ARRAY = 0x00002000,
            FORMAT_MESSAGE_FROM_HMODULE = 0x00000800,
            FORMAT_MESSAGE_FROM_STRING = 0x00000400,
        }
        #endregion

        // Gets a user friendly string message for a system error code

        public string GetSystemMessage(int errorCode)
        {
            try
            {
                IntPtr lpMsgBuf = IntPtr.Zero;

                int dwChars = FormatMessage(
                    FormatMessageFlags.FORMAT_MESSAGE_ALLOCATE_BUFFER | FormatMessageFlags.FORMAT_MESSAGE_FROM_SYSTEM | FormatMessageFlags.FORMAT_MESSAGE_IGNORE_INSERTS,
                    IntPtr.Zero,
                    (uint)errorCode,
                    0,
                    ref lpMsgBuf,
                    0,
                    IntPtr.Zero);
                if (dwChars == 0)
                {
                    int le = Marshal.GetLastWin32Error();
                    return "Unable to get error code string from System - Error " + le.ToString();
                }

                string sRet = Marshal.PtrToStringAnsi(lpMsgBuf);

                lpMsgBuf = LocalFree(lpMsgBuf);
                return sRet;
            }
            catch (Exception e)
            {
                return "Unable to get error code string from System -> " + e.ToString();
            }
        }
    }

    //    public Dictionary<int, string> error_list = new Dictionary<int, string>();

    //    public void readErrorCode(int error_code)
    //    {
    //        if (error_code == 2)
    //        {
    //            error_list.Add(2, "The system cannot find the file specified.");
    //        }
    //        else if (error_code == 3)
    //        {
    //            error_list.Add(3, "The system cannot find the path specified.");
    //        }
    //        else if (error_code == 6)
    //        {
    //            error_list.Add(6, "The handle is invalid.");
    //        }
    //        else if (error_code == 5)
    //        {
    //            error_list.Add(5, "Access is denied.");
    //        }
    //        else if (error_code == 23)
    //        {
    //            error_list.Add(23, "Data error (cyclic redundancy check).");
    //        }
    //        else if (error_code == 129)
    //        {
    //            error_list.Add(129, "The %1 application cannot be run in Win32 mode.");
    //        }
    //        else
    //        {
    //            error_list.Add(error_code, "The dll is not a managed assambly.");
    //        }
    //    }
    //}
}
