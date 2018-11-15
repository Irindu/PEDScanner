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
        public void readErrorCode(int error_code)
        {
            if (error_code == 2)
            {
                Console.WriteLine("The system cannot find the file specified.");
            }
            else if (error_code == 3)
            {
                Console.WriteLine("The system cannot find the path specified.");
            }
            else if(error_code == 6)
            {
                Console.WriteLine("The handle is invalid.");
            }
            else if (error_code == 5)
            {
                Console.WriteLine("Access is denied.");
            }
            else if (error_code == 23)
            {
                Console.WriteLine("Data error (cyclic redundancy check).");
            }
            else if (error_code == 129)
            {
                Console.WriteLine("The %1 application cannot be run in Win32 mode.");
            }
            else 
            {
                Console.WriteLine(error_code + "The dll is not a managed assambly");
            }
        }
    }
}
