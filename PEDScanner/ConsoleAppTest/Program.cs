using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using PEDScannerLib.Core;
using System.IO;
using PEDScannerLib;
using PEDScannerLib.Objects;



namespace PEDScannerConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"E://Project1.exe";

            string fileName = Path.GetFileName(path);
            Console.WriteLine("Hello World!");
            List<string> listWithRoot = new List<string>
            {
                fileName
            };
            PortableExecutable PE;
            
            PE = new PortableExecutable(fileName, path,true, listWithRoot);

            unsafe
            {

                List<PortableExecutable> pe = PE.Dependencies;
                foreach (PortableExecutable porte in pe)
                {
                    Console.WriteLine("dependency Path={0}, dependency name={1}, is loaded={2}", porte.FilePath, porte.Name, porte.IsLoadable);
                }
                

                List<ImportFunctionObject> importFunctions = PE.ImportFunctions;
                foreach (ImportFunctionObject import in importFunctions)
                {
                    Console.WriteLine("import function= {0}, Address ={1}, dependency = {2}", import.Function, import.BaseAddress, import.Dependency);
                }


                List<FunctionObject> exportedFunctions = PE.ExportedFunctions;
                foreach (FunctionObject export in exportedFunctions)
                {
                    Console.WriteLine("exports={0}", export.Function);
                }
                List<HeaderObject> headers = PE.Headers;

                foreach (HeaderObject header in headers)
                {
                    Console.WriteLine("header Name={0}, value={1}", header.Name, header.Value);
                }

                // List<SectionObject> sections = PE.GetSections();
                List<DirectoryObject> directories = PE.Directories;
                foreach (DirectoryObject directory in directories)
                {
                    Console.WriteLine("name={0}, virtual ={1}, length={2}", directory.Name, directory.RVA, directory.Size);
                }
                List<SectionObject> sections = PE.Sections;
                foreach (SectionObject section in sections)
                {
                    Console.WriteLine("nameOfSection={0}, virtualAddress={1}, virtualSize={2}, pointerToRawData={3}, sizeOFrawData={4}", section.Name, section.VirtualAddress, section.VirtualSize, section.RawDataOffset, section.RawDataSize);

                }

            }

            Console.ReadKey();
        }

     
    }
}
