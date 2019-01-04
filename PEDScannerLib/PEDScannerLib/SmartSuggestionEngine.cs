using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PEDScannerLib
{
    public class SmartSuggestionEngine
    {
        public Dictionary<int, string> error_list = new Dictionary<int, string>();

        public void readErrorCode(int error_code)
        {
            if (error_code == 2)
            {
                error_list.Add(2, "The system cannot find the file specified.");
            }
            else if (error_code == 3)
            {
                error_list.Add(3, "The system cannot find the path specified.");
            }
            else if (error_code == 6)
            {
                error_list.Add(6, "The handle is invalid.");
            }
            else if (error_code == 5)
            {
                error_list.Add(5, "Access is denied.");
            }
            else if (error_code == 23)
            {
                error_list.Add(23, "Data error (cyclic redundancy check).");
            }
            else if (error_code == 129)
            {
                error_list.Add(129, "The %1 application cannot be run in Win32 mode.");
            }
            else
            {
                error_list.Add(error_code, "The dll is not a managed assambly.");
            }
        }
    }
}
