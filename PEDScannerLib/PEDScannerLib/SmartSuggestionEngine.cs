using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using PEDScannerLib.Core;

namespace PEDScannerLib
{
    public class SmartSuggestionEngine
    {

        //public List<Suggestions> suggestionObjects()
        //{

        //    return suggestions_list;
        //}        


        //}

        //public class Suggestions
        //{

        ////Suggestions suggestions = new Suggestions();
        //public Dictionary<uint, string> suggestion_list = new Dictionary<uint, string>();

        //#region definitions
        //[DllImport("kernel32.dll", SetLastError = true)]
        //static extern IntPtr LocalFree(IntPtr hMem);

        //[DllImport("kernel32.dll", SetLastError = true)]
        //static extern int FormatMessage(FormatMessageFlags dwFlags, IntPtr lpSource, uint dwMessageId, uint dwLanguageId, ref IntPtr lpBuffer, uint nSize, IntPtr Arguments);

        //[Flags]
        //private enum FormatMessageFlags : uint
        //{
        //    FORMAT_MESSAGE_ALLOCATE_BUFFER = 0x00000100,
        //    FORMAT_MESSAGE_IGNORE_INSERTS = 0x00000200,
        //    FORMAT_MESSAGE_FROM_SYSTEM = 0x00001000,
        //    FORMAT_MESSAGE_ARGUMENT_ARRAY = 0x00002000,
        //    FORMAT_MESSAGE_FROM_HMODULE = 0x00000800,
        //    FORMAT_MESSAGE_FROM_STRING = 0x00000400,
        //}
        //#endregion

        //// Gets a user friendly string message for a system error code

        //public void GetSystemMessage(uint errorCode)
        //{
        //    try
        //    {
        //        IntPtr lpMsgBuf = IntPtr.Zero;

        //        int dwChars = FormatMessage(
        //            FormatMessageFlags.FORMAT_MESSAGE_ALLOCATE_BUFFER | FormatMessageFlags.FORMAT_MESSAGE_FROM_SYSTEM | FormatMessageFlags.FORMAT_MESSAGE_IGNORE_INSERTS,
        //            IntPtr.Zero,
        //            (uint)errorCode,
        //            0,
        //            ref lpMsgBuf,
        //            0,
        //            IntPtr.Zero);
        //        if (dwChars == 0)
        //        {
        //            int le = Marshal.GetLastWin32Error();
        //            suggestion_list.Add(0, "Unable to get error code string from System - Error " + le.ToString());
        //        }

        //        string sRet = Marshal.PtrToStringAnsi(lpMsgBuf);

        //        lpMsgBuf = LocalFree(lpMsgBuf);
        //        suggestion_list.Add(errorCode, sRet);
        //    }
        //    catch (Exception e)
        //    {
        //        suggestion_list.Add(0, "Unable to get error code string from System -> " + e.ToString());
        //    }
        //}

        public Dictionary<string, List<string>> error_list = new Dictionary<string, List<string>>();

        public void readErrorCode(string name, int error_code)
        {
            //if (error_code == 2)
            //{
            //    if (!error_list.ContainsKey(name))
            //    {
            //        List<string> errors = new List<string>();
            //        errors.Add("Try to load unmanaged assembly.");
            //        error_list.Add(name, errors);
            //    }
            //    else
            //    {
            //        List<string> errorExisting = new List<string>();
            //        error_list.TryGetValue(name, out errorExisting);
            //        errorExisting.Add("Try to load unmanaged assembly.");
            //        //error_list.Add(name, errorExisting);
            //    }
            //}
            //else
        if (error_code == 3)
            {
                if (!error_list.ContainsKey(name))
                {
                    List<string> errors = new List<string>();
                    errors.Add("File path is null when  get assembly dependencies.");
                    error_list.Add(name, errors);
                }
                else
                {
                    List<string> errorExisting = new List<string>();
                    error_list.TryGetValue(name, out errorExisting);
                    errorExisting.Add("File path is null when  get assembly dependencies.");
                    error_list.Add(name, errorExisting);
                }
            }
            else if (error_code == 4)
            {
                if (!error_list.ContainsKey(name))
                {
                    List<string> errors = new List<string>();
                    errors.Add("Import, Export mismatch error in " + name);
                    error_list.Add(name, errors);
                }
                else
                {
                    List<string> errorExisting = new List<string>();
                    error_list.TryGetValue(name, out errorExisting);
                    errorExisting.Add("Import, Export mismatch error" + name);
                    error_list.Add(name, errorExisting);
                }
            }
            else if (error_code == 5)
            {
                if (!error_list.ContainsKey(name))
                {
                    List<string> errors = new List<string>();
                    errors.Add("The file path is null in " + name);
                    error_list.Add(name, errors);
                }
                else
                {
                    List<string> errorExisting = new List<string>();
                    error_list.TryGetValue(name, out errorExisting);
                    errorExisting.Add("The file path is null in " + name);
                    error_list.Add(name, errorExisting);
                }
            }
            else if (error_code == 6)
            {
                if (!error_list.ContainsKey(name))
                {
                    List<string> errors = new List<string>();
                    errors.Add("Circular dependency triggered in " + name);
                    error_list.Add(name, errors);
                }
                else
                {
                    List<string> errorExisting = new List<string>();
                    error_list.TryGetValue(name, out errorExisting);
                    errorExisting.Add("Erro occured when loading imports in " + name);
                    error_list.Add(name, errorExisting);
                }
            }
            else if (error_code == 7)
            {
                if (!error_list.ContainsKey(name))
                {
                    List<string> errors = new List<string>();
                    errors.Add("Error occured when loading imports in " + name);
                    error_list.Add(name, errors);
                }
                else
                {
                    List<string> errorExisting = new List<string>();
                    error_list.TryGetValue(name, out errorExisting);
                    errorExisting.Add("Error occured when loading imports in " + name);
                    error_list.Add(name, errorExisting);
                }
            }
            else if(error_code == 8)
            {
                if (!error_list.ContainsKey(name))
                {
                    List<string> errors = new List<string>();
                    errors.Add("Error occured when loading exports in " + name);
                    error_list.Add(name, errors);
                }
                else
                {
                    List<string> errorExisting = new List<string>();
                    error_list.TryGetValue(name, out errorExisting);
                    errorExisting.Add("Error occured when loading exports in " + name);
                    error_list.Add(name, errorExisting);
                }
            }
        }
    }

}