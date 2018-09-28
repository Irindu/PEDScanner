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
            PortableExecutable PE;
            
            PE = new PortableExecutable(fileName, path,true);
            
            unsafe
            {
            
                List<PortableExecutable> pe = PE.MakeDependencies();
                foreach (PortableExecutable porte in pe)
                {
                    Console.WriteLine("dependency Path={0}, dependency name={1}, is loaded={2}", porte.FilePath, porte.Name, porte.IsLoadable);
                }
                //List<DependeciesObject> dependencies2 = PE.FindDependencies();
                //foreach (DependeciesObject dependencyobject in dependencies2)
                //{
                //    Console.WriteLine("Name = {0},Isloaded={1}", dependencyobject.dependencyName, dependencyobject.isLoadable);
                //}

                List<ImportFunctionObject> importFunctions = PE.LoadImports(path, true);
                foreach (ImportFunctionObject import in importFunctions)
                {
                    Console.WriteLine("import function= {0}, Address ={1}, dependency = {2}", import.function, import.baseAddress, import.dependency);
                }


                List<FunctionObject> exportedFunctions = PE.LoadExports(path, true);
                foreach (FunctionObject export in exportedFunctions)
                {
                    Console.WriteLine("exports={0}", export.function);
                }
                List<HeaderObject> headers = PE.GetHeader();

                foreach (HeaderObject header in headers)
                {
                    Console.WriteLine("header Name={0}, value={1}", header.name, header.value);
                }

                // List<SectionObject> sections = PE.GetSections();
                List<DirectoryObject> directories = PE.GetDirectories();
                foreach (DirectoryObject directory in directories)
                {
                    Console.WriteLine("name={0}, virtual ={1}, length={2}", directory.name, directory.RVA, directory.Size);
                }
                List<SectionObject> sections = PE.GetSections();
                foreach (SectionObject section in sections)
                {
                    Console.WriteLine("nameOfSection={0}, virtualAddress={1}, virtualSize={2}, pointerToRawData={3}, sizeOFrawData={4}", section.name, section.virtualAddress, section.virtualsize, section.rawDataOffset, section.rawDataSize);

                }

            }
           
            Console.ReadKey();
        }

     
    }
}
