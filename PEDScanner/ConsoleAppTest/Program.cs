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

using Objects;
using static System.Net.Mime.MediaTypeNames;
using System.Reflection;

namespace PEDScannerConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
           
            string path = @"C:\Program Files\Git\bin\git.exe";
           
            string fileName = Path.GetFileName(path);
            Console.WriteLine("Hello World!");
           
           
            List<string> listWithRoot = new List<string>
            {
                fileName
            };
            PortableExecutable PE;

            PE = new PortableExecutable(fileName, path, true, listWithRoot);
            PortableExecutableLoader portableExecutableLoader = new PortableExecutableLoader();
            portableExecutableLoader.Load(PE);

            unsafe
            {
                List<string> importName = PE.ImportNames;
                foreach (string importname in importName)
                {
                    Console.WriteLine(importname);
                }



                List<ImportFunctionObject> importFunctions = PE.ImportFunctions;
                foreach (ImportFunctionObject import in importFunctions)
                {
                    Console.WriteLine("import function= {0}, Address ={1}, dependency = {2}", import.function, import.baseAddress, import.dependency);
                }
                List<PortableExecutable> depen = PE.Dependencies;
                foreach (PortableExecutable dep in depen)
                {
                    Console.WriteLine(dep.DependencyNames);
                }


                List<FunctionObject> exportedFunctions = PE.ExportedFunctions;
                foreach (FunctionObject export in exportedFunctions)
                {
                    Console.WriteLine("exports={0}", export.function);
                }
                List<HeaderObject> headers = PE.Headers;

                foreach (HeaderObject header in headers)
                {
                    Console.WriteLine("header Name={0}, value={1}", header.name, header.value);
                }

                // List<SectionObject> sections = PE.GetSections();
                List<DirectoryObject> directories = PE.Directories;
                foreach (DirectoryObject directory in directories)
                {
                    Console.WriteLine("name={0}, virtual ={1}, length={2}", directory.name, directory.RVA, directory.Size);
                }
                List<SectionObject> sections = PE.Sections;
                foreach (SectionObject section in sections)
                {
                    Console.WriteLine("nameOfSection={0}, virtualAddress={1}, virtualSize={2}, pointerToRawData={3}, sizeOFrawData={4}", section.name, section.virtualAddress, section.virtualsize, section.rawDataOffset, section.rawDataSize);

                }

            }

            Console.ReadKey();
        }


    }
}
