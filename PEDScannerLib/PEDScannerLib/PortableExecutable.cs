using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.IO;
using System.Security;
using System.Reflection;
using PEDScannerLib.Struct;
using PEDScannerLib.Objects;
using System.Collections;
using Objects;

namespace PEDScannerLib.Core
{
    /// <summary>
    /// Reads in the header information of the Portable Executable format.
    /// </summary>
    /// 

    public class PortableExecutable
    {
        const uint DONT_RESOLVE_DLL_REFERENCES = 0x00000001;
        const uint LOAD_IGNORE_CODE_AUTHZ_LEVEL = 0x00000010;

        [DllImport("kernel32.dll"), SuppressUnmanagedCodeSecurity]
        static extern uint LoadLibraryEx(string fileName, uint notUsedMustBeZero, uint flags);

        [DllImport("kernel32", SetLastError = true)]
        static extern IntPtr LoadLibrary(string lpFileName);

        public string Name { get; set; }
        public string FilePath;
        public PeHeaderReader reader;
        public bool IsLoadable;
        public List<PortableExecutable> Dependencies;
        public List<DependeciesObject> DependencyNames;
        public List<FunctionObject> ExportedFunctions;
        public List<ImportFunctionObject> ImportFunctions;
        public List<HeaderObject> Headers;
        public List<SectionObject> Sections;
        public List<DirectoryObject> Directories;
        public List<string> ImportNames;
        public List<string> listOfBranch;
        public Assembly assembly;
        public List<String> importMismatchedFiles;
        public List<string> circularDependencyFiles;
        public List<ErrorObject> issues;
      //  static Hashtable filePathsTable = new Hashtable();

        public string directoryPath = Directory.GetCurrentDirectory();
       
        public PortableExecutable(String Name, string FilePath)
        {
            this.Name = Name;
            this.FilePath = FilePath;
            this.IsLoadable = true;
            this.listOfBranch = new List<string>();

            // reader = new PeHeaderReader(FilePath);
            ExportedFunctions = new List<FunctionObject>();
            ImportFunctions = new List<ImportFunctionObject>();
            Headers = new List<HeaderObject>();
            Sections = new List<SectionObject>();
            Directories = new List<DirectoryObject>();
            ImportNames = new List<string>();
            DependencyNames = new List<DependeciesObject>();
            Dependencies = new List<PortableExecutable>();
            issues = new List<ErrorObject>();
        }


        public PortableExecutable(string Name, string FilePath, bool IsLoadable, List<string> listOfBranches)
        {
            this.Name = Name;
            this.FilePath = FilePath;
            this.IsLoadable = IsLoadable;
            this.listOfBranch = listOfBranches;

            // reader = new PeHeaderReader(FilePath);
            ExportedFunctions = new List<FunctionObject>();
            ImportFunctions = new List<ImportFunctionObject>();
            Headers = new List<HeaderObject>();
            Sections = new List<SectionObject>();
            Directories = new List<DirectoryObject>();
            ImportNames = new List<string>();
            DependencyNames = new List<DependeciesObject>();
            Dependencies = new List<PortableExecutable>();
            importMismatchedFiles = new List<string>();
            circularDependencyFiles = new List<string>();
            issues = new List<ErrorObject>();
        }

        public override bool Equals(object obj)
        {
            var item = obj as PortableExecutable;

            if (item == null)
            {
                return false;
            }

            return this.Name.Equals(item.Name);
        }
    }
    public class PortableExecutableLoader
        {

            const uint DONT_RESOLVE_DLL_REFERENCES = 0x00000001;
            const uint LOAD_IGNORE_CODE_AUTHZ_LEVEL = 0x00000010;
             static Hashtable filePathsTable = new Hashtable();
            [DllImport("kernel32.dll"), SuppressUnmanagedCodeSecurity]
            static extern uint LoadLibraryEx(string fileName, uint notUsedMustBeZero, uint flags);

            [DllImport("kernel32", SetLastError = true)]
            static extern IntPtr LoadLibrary(string lpFileName);

             [DllImport("kernel32.dll")]
                static extern uint GetLastError();

            [DllImport("kernel32.dll", SetLastError = true)]
             static extern bool SetVolumeLabel(string lpRootPathName, string lpVolumeName);
             public SmartSuggestionEngine smartSuggestionEngine;

            public PortableExecutableLoader()
            {
            smartSuggestionEngine = new SmartSuggestionEngine();
            }


            public void Load(PortableExecutable portableExecutable)
            {
                //references to properties of the portableExecutable we are going to load
                String FilePath = portableExecutable.FilePath;
                String currentDirectory = portableExecutable.directoryPath;
                List<string> listOfBranch = portableExecutable.listOfBranch;
                List<PortableExecutable> Dependencies = portableExecutable.Dependencies;
                List<DependeciesObject> DependencyNames = portableExecutable.DependencyNames;
                List<FunctionObject> ExportedFunctions = portableExecutable.ExportedFunctions;
                List<ImportFunctionObject> ImportFunctions = portableExecutable.ImportFunctions;
                List<HeaderObject> Headers = portableExecutable.Headers;
                List<SectionObject> Sections = portableExecutable.Sections;
                List<DirectoryObject> Directories = portableExecutable.Directories;
                List<string> ImportNames = portableExecutable.ImportNames;
                List<string> importMismatchedFiles = portableExecutable.importMismatchedFiles;
                List<string> circularDependencyFiles = portableExecutable.circularDependencyFiles;
                List<ErrorObject> issues = portableExecutable.issues;
            //the PE Header reader to be used 
            PeHeaderReader reader = new PeHeaderReader(FilePath);
                if (Is32bitFile(reader))
                {
                LoadImports(FilePath, true, ImportFunctions, ImportNames);
                LoadExports(FilePath, true, ExportedFunctions);
                 }
                 else
                  {
                Service64Proxy.Service64 proxy = new Service64Proxy.Service64();
                MyObject obj2 = new MyObject();
                MyObject myobject = proxy.Load64Imports(obj2, FilePath, true);
                List<ImportFunctionObject> listOfobj = myobject.FunctionObjectList;
                foreach (ImportFunctionObject importFunctions in listOfobj)
                {
                    ImportFunctions.Add(importFunctions);
                    ImportNames.Add(importFunctions.Dependency);
                }
                ExportObject exportObject = new ExportObject();
                ExportObject exports = proxy.Load64Exports(exportObject, FilePath, true);
                List<FunctionObject> exportList = exports.ExportFunctionObjectList;
                ExportedFunctions.AddRange(exportList);
                    }

                GetHeader(Headers, reader);
                GetAssemblyDependencies(FilePath, ImportFunctions, ImportNames); 
                GetAssemblyExports(FilePath, ExportedFunctions);
                GetDirectories(Directories, reader);
                GetSections(Sections, reader);
                LoadDependencies(ImportNames, Dependencies, currentDirectory, FilePath, reader, listOfBranch, this, ImportFunctions, importMismatchedFiles, circularDependencyFiles,issues);
              


        }

        /// <summary>
        /// Check whether the library is loaded succefully or not.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        static bool CheckLibrary(string fileName)
        {
            return LoadLibrary(fileName) == IntPtr.Zero;
        }


        /// <summary>
        /// Load each of the dependencies as a Portable Executable Object
        /// </summary>
        /// <returns> The Dependencies in a Portable Executable File Format </returns>
        private void LoadDependencies(List<string> ImportNames, List<PortableExecutable> Dependencies, String currentDirectory, String FilePath, PeHeaderReader reader, List<string> listOfBranch, PortableExecutableLoader portableExecutableLoader, List<ImportFunctionObject> importFunctions,List<string> importMismatchedFiles, List<string> circularDependencyFiles, List<ErrorObject> issues)
        {
            PortableExecutable PE;
            string filePath;
           
            unsafe
            {
                try
                {
                    ImportNames = ImportNames.Distinct().ToList();
                    foreach (string name in ImportNames)
                    {
                        filePath = null;
                       
                        if (!filePathsTable.ContainsKey(name))
                        {
                            var importedFunction = from import in importFunctions
                                                   where import.Dependency == name
                                                   select import.Function;
                            List<String> importedFunctionNames = new List<string>();
                            importedFunctionNames.AddRange(importedFunction);

                           

                            foreach (String path in GetModulePath(name, currentDirectory, FilePath, reader))
                            {

                                List<FunctionObject> exportsOfFile = new List<FunctionObject>();
                               
                                List<string> exportsFunctionNames = new List<string>();
                                if (path != null)
                                {
                                    LoadExports(path, true, exportsOfFile);
                                    foreach (FunctionObject exports in exportsOfFile)
                                    {
                                        exportsFunctionNames.Add(exports.Function);
                                    }
                                    var firstNotSecond = importedFunctionNames.Except(exportsFunctionNames).ToList();
                                    //   filePath = path;
                                    if (string.Equals(name, "kernel32.dll", StringComparison.OrdinalIgnoreCase) || string.Equals(name, "msvcrt.dll", StringComparison.OrdinalIgnoreCase))
                                    {
                                        filePath = GetModulePath(name, currentDirectory, FilePath, reader).First();
                                        break;
                                    }
                                    else if (!importedFunctionNames.Except(exportsFunctionNames).Any())
                                    {
                                        filePath = path;
                                        break;
                                    }
                                }
                            }
                        }

                        else
                        {
                            filePath = filePathsTable[name].ToString();
                        }

                        if (filePath == null)
                        {
                            if (GetModulePath(name, currentDirectory, FilePath, reader).Count > 0)
                            {
                             //   importMismatchedFiles.Add(name);
                                issues.Add(new ErrorObject(name,"Imports and Export mismatch found"));
                                //Suggestion for import export mismatch error
                                 smartSuggestionEngine.readErrorCode(name, 4);
                            }
                            else
                            {
                                issues.Add(new ErrorObject(name, "File path not found"));
                                //Suggestion for null file path
                                 smartSuggestionEngine.readErrorCode(name, 5);
                            }
                        }
                        var hLib2 = LoadLibraryEx(filePath, 0,
                                              DONT_RESOLVE_DLL_REFERENCES | LOAD_IGNORE_CODE_AUTHZ_LEVEL);
                        if (listOfBranch.Contains(name))
                        {
                          //  circularDependencyFiles.Add(name);
                            issues.Add(new ErrorObject(name, "Circular dependency is detected"));
                            //Suggestion for circular dependency
                            smartSuggestionEngine.readErrorCode(name, 6);
                            continue;
                        }
                        else
                        {
                            List<string> newBranchList = listOfBranch.ToList();
                            newBranchList.Add(name);
                            if (!CheckLibrary(name))
                            {
                                PE = new PortableExecutable(name, filePath, true, newBranchList);
                            }
                            else
                            {
                                PE = new PortableExecutable(name, filePath, false, newBranchList);

                            }
                           // Dependencies.Add(PE);
                            portableExecutableLoader.Load(PE);
                            Dependencies.Add(PE);
                        }

                    }
                }
                //To catch error in dependencies
                catch (Exception) {
                  
                }
                return;
            }
        }

        private void GetAssemblyExports(string FilePath, List<FunctionObject> Exports)
        {
            Assembly assembly;
            if (FilePath != null)
            {
                if (IsManagedAssembly(FilePath))
                {
                    assembly = Assembly.LoadFrom(FilePath);

                    var exports =
                    assembly.GetExportedTypes()
                            .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.Public)
                                                    .Select(member => new
                                                    {
                                                        // type.FullName
                                                        Member = member.ToString()
                                                    }))
                            .ToList();
                    for (int i = 0; i < exports.Count; i++)
                    {
                        Exports.Add(new FunctionObject(exports[i].Member.ToString()));
                    }
                }


            }
        }
        /// <summary>
        /// find the .Net dependencies for managed dlls.
        /// input is file path of the file and output is .Net dependencies as ImportFunctionObject 
        /// </summary>
        /// <param name="FilePath"></param>
        /// <returns></returns>

        private void GetAssemblyDependencies(string FilePath, List<ImportFunctionObject> ImportFunctions, List<string> ImportNames)
        {
            Assembly assembly;
            if (FilePath != null)
            {
                if (IsManagedAssembly(FilePath))
                {
                    assembly = Assembly.LoadFrom(FilePath);
                    Dictionary<Type, List<MethodInfo>> exports = assembly.GetTypes()
                      .SelectMany(type1 => type1.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Static)
                                              .Where(method => method.GetCustomAttributes(typeof(DllImportAttribute), false).Length > 0))
                      .GroupBy(method => method.DeclaringType)
                      .ToDictionary(item => item.Key, item => item.ToList())
                      ;

                    foreach (var item in exports)
                    {
                        foreach (var method in item.Value)
                        {

                            DllImportAttribute attr = method.GetCustomAttributes(typeof(DllImportAttribute), false)[0] as DllImportAttribute;
                            if (!attr.Value.Contains("api-ms-win"))
                            {
                                ImportFunctions.Add(new ImportFunctionObject(method.Name, 0, attr.Value));
                                ImportNames.Add(attr.Value);
                            }
                        }
                    }

                    // Find the set of assemblies our assemblies reference
                    foreach (AssemblyName an in assembly.GetReferencedAssemblies())
                    {
                        if (!an.Name.Contains("api-ms-win"))
                        {
                            ImportNames.Add((an.Name + ".dll"));
                        }
                    }
                    //find file path for assembly dependencies
                    foreach (Assembly b in AppDomain.CurrentDomain.GetAssemblies())
                    {
                        string codeBase = b.CodeBase;
                        UriBuilder uri = new UriBuilder(codeBase);
                        string pathOfAssembly = Uri.UnescapeDataString(uri.Path);
                        string assemblyName = Path.GetFileName(pathOfAssembly);
                        if (!filePathsTable.ContainsKey(assemblyName))
                        {
                            filePathsTable.Add(assemblyName, pathOfAssembly);
                        }
                    }
                }
                //else
                //{
                //    smartSuggestionEngine.readErrorCode("basic error 1", 2);
                //}
            }
            //else
            //{
            //    smartSuggestionEngine.readErrorCode("basic error 2", 3);
            //}
            return;
        }

        /// <summary>
        /// find the header information
        /// </summary>
        /// <returns></returns>
        private void GetHeader(List<HeaderObject> Headers, PeHeaderReader reader)
        {
            IMAGE_FILE_HEADER fileHeader = reader.FileHeader;
            UInt16 machine = fileHeader.Machine;
            UInt16 numberOfSections = fileHeader.NumberOfSections;
            UInt32 timeDateStamp = fileHeader.TimeDateStamp;
            UInt32 pointerToSymbolTable = fileHeader.PointerToSymbolTable;
            UInt32 numberOfSymbols = fileHeader.NumberOfSymbols;
            UInt16 sizeOfOptionalHeader = fileHeader.SizeOfOptionalHeader;
            UInt16 characteristics = fileHeader.Characteristics;
            string MachineType = GetMachineType(reader);
            string character = GetCharacterInformation(characteristics);
            Headers.Add(new HeaderObject("Machine", MachineType));
            Headers.Add(new HeaderObject("Number of sections", numberOfSections.ToString()));
            Headers.Add(new HeaderObject("Timestamp", timeDateStamp.ToString()));
            Headers.Add(new HeaderObject("Pointer to symbol table", pointerToSymbolTable.ToString()));
            Headers.Add(new HeaderObject("Number of symbols", numberOfSymbols.ToString()));
            Headers.Add(new HeaderObject("Size of optional header", sizeOfOptionalHeader.ToString()));
            Headers.Add(new HeaderObject("Characteristics", character));
            return;
        }

        /// <summary>
        /// find the section information for a given file
        /// </summary>
        /// <returns></returns>
        private void GetSections(List<SectionObject> Sections, PeHeaderReader reader)
        {
            PeHeaderReader.IMAGE_SECTION_HEADER[] sections = reader.ImageSectionHeaders;
            IMAGE_FILE_HEADER fileheader = reader.FileHeader;
            UInt32 numberofSection = fileheader.NumberOfSections;
            foreach (PeHeaderReader.IMAGE_SECTION_HEADER section in sections)
            {
                char[] name = section.Name;
                UInt32 virtualAddress = section.VirtualAddress;
                UInt32 pointerToRawData = section.PointerToRawData;
                UInt32 virtualSize = section.VirtualSize;
                UInt32 sizeOFrawData = section.SizeOfRawData;
                UInt32 pointerToRelocations = section.PointerToRelocations;
                UInt32 pointerToLineNumber = section.PointerToLinenumbers;
                UInt16 numberOfRelocations = section.NumberOfRelocations;
                UInt16 NumberOfLineNumbers = section.NumberOfLinenumbers;
                PeHeaderReader.DataSectionFlags dataSectionFlags = section.Characteristics;
                string nameOfSection = section.Section;
               
                Sections.Add(new SectionObject(nameOfSection, virtualAddress, virtualSize, pointerToRawData, sizeOFrawData));
            }
            return;
        }

        /// <summary>
        /// find the directories
        /// </summary>
        /// <returns></returns>
        private void GetDirectories(List<DirectoryObject> Directories, PeHeaderReader reader)
        {
            unsafe
            {
                if (reader.Is32BitHeader)
                {
                    PeHeaderReader.IMAGE_OPTIONAL_HEADER32 header32 = reader.OptionalHeader32;
                    UInt32 sizeOfHeaders = header32.SizeOfHeaders;
                    PeHeaderReader.IMAGE_DATA_DIRECTORY ImportTable = header32.ImportTable;
                    PeHeaderReader.IMAGE_DATA_DIRECTORY ExportTable = header32.ExportTable;
                    PeHeaderReader.IMAGE_DATA_DIRECTORY Resources = header32.ResourceTable;
                    PeHeaderReader.IMAGE_DATA_DIRECTORY loadConfiguration = header32.LoadConfigTable;
                    PeHeaderReader.IMAGE_DATA_DIRECTORY delayLoadImport = header32.DelayImportDescriptor;
                    PeHeaderReader.IMAGE_DATA_DIRECTORY Debug = header32.Debug;
                    PeHeaderReader.IMAGE_DATA_DIRECTORY BaseRelocation = header32.BaseRelocationTable;
                    Directories.Add(new DirectoryObject("Import Table", ImportTable.VirtualAddress, ImportTable.Size));
                    Directories.Add(new DirectoryObject("Export Table", ExportTable.VirtualAddress, ExportTable.Size));
                    Directories.Add(new DirectoryObject("Resource Table", Resources.VirtualAddress, Resources.Size));
                    Directories.Add(new DirectoryObject("Load Configuration", loadConfiguration.VirtualAddress, loadConfiguration.Size));
                    Directories.Add(new DirectoryObject("Delay Load Import", delayLoadImport.VirtualAddress, delayLoadImport.Size));
                }
                else
                {
                    PeHeaderReader.IMAGE_OPTIONAL_HEADER64 header64 = reader.OptionalHeader64;
                    UInt64 sizeOfHeaders = header64.SizeOfHeaders;
                    PeHeaderReader.IMAGE_DATA_DIRECTORY ImportTable = header64.ImportTable;
                    PeHeaderReader.IMAGE_DATA_DIRECTORY ExportTable = header64.ExportTable;
                    PeHeaderReader.IMAGE_DATA_DIRECTORY Resources = header64.ResourceTable;
                    PeHeaderReader.IMAGE_DATA_DIRECTORY loadConfiguration = header64.LoadConfigTable;
                    PeHeaderReader.IMAGE_DATA_DIRECTORY ImportAddressTable = header64.BoundImport;
                    PeHeaderReader.IMAGE_DATA_DIRECTORY delayLoadImport = header64.DelayImportDescriptor;
                    Directories.Add(new DirectoryObject("Import Table", ImportTable.VirtualAddress, ImportTable.Size));
                    Directories.Add(new DirectoryObject("Export Table", ExportTable.VirtualAddress, ExportTable.Size));
                    Directories.Add(new DirectoryObject("Resource Table", Resources.VirtualAddress, Resources.Size));
                    Directories.Add(new DirectoryObject("Load Configuration", loadConfiguration.VirtualAddress, loadConfiguration.Size));
                    Directories.Add(new DirectoryObject("Delay Load Import", delayLoadImport.VirtualAddress, delayLoadImport.Size));
                }
            }
            return;
        }

        // using mscoree.dll as an example as it doesnt export any thing
        // so nothing shows up when use the own module.
        // and the only none delayload in mscoree.dll is the Kernel32.dll
        /// <summary>
        /// return the imported dlls and functions from them
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="mappedAsImage"></param>
        /// <returns></returns>
        private void LoadImports(string filePath, bool mappedAsImage, List<ImportFunctionObject> ImportFunctions, List<String> ImportNames)
        {
            var hLib = LoadLibrary(filePath);
            if (hLib == null)
            {
                var errorCode = GetLastError();
            }
            //var hLib = LoadLibraryEx(filePath, 0,
            //                   DONT_RESOLVE_DLL_REFERENCES | LOAD_IGNORE_CODE_AUTHZ_LEVEL);
          
            unsafe
            {
                {
                    void* hMod = (void*)hLib;
                    uint size = 0;
                    uint BaseAddress = (uint)hMod;
                    if (hMod != null)
                    {
                        IMAGE_IMPORT_DESCRIPTOR* pIID = (IMAGE_IMPORT_DESCRIPTOR*)Interop.ImageDirectoryEntryToData((void*)hMod, mappedAsImage, Interop.IMAGE_DIRECTORY_ENTRY_IMPORT, out size);
                        if (pIID != null)
                        {
                            //walk the array until find the end of the array
                            while (pIID->OriginalFirstThunk != 0)
                            {
                                try
                                {
                                    //Name contains the RVA to the name of the dll. 
                                    //Thus convert it to a virtual address first.
                                    char* szName = (char*)(BaseAddress + pIID->Name);
                                    string name = Marshal.PtrToStringAnsi((IntPtr)szName);
                                    if (!name.Contains("api-ms-win"))
                                    {
                                        ImportNames.Add(name);
                                        // value in OriginalFirstThunk is an RVA. 
                                        // convert it to virtual address.
                                        THUNK_DATA* pThunkOrg = (THUNK_DATA*)(BaseAddress + pIID->OriginalFirstThunk);
                                        while (pThunkOrg->AddressOfData != 0)
                                        {
                                            char* szImportName;
                                            uint Ord;

                                            if ((pThunkOrg->Ordinal & 0x80000000) > 0)
                                            {
                                                Ord = pThunkOrg->Ordinal & 0xffff;
                                            }
                                            else
                                            {
                                                IMAGE_IMPORT_BY_NAME* pIBN = (IMAGE_IMPORT_BY_NAME*)(BaseAddress + pThunkOrg->AddressOfData);

                                                if (!Interop.IsBadReadPtr((void*)pIBN, (uint)sizeof(IMAGE_IMPORT_BY_NAME)))
                                                {
                                                    Ord = pIBN->Hint;
                                                    szImportName = (char*)pIBN->Name;
                                                    string sImportName = Marshal.PtrToStringAnsi((IntPtr)szImportName);

                                                    UInt32 Address = pThunkOrg->Function;

                                                    ImportFunctions.Add(new ImportFunctionObject(sImportName, Address, name));
                                                }
                                                else
                                                {
                                                    break;
                                                }
                                            }
                                            pThunkOrg++;
                                        }
                                    }
                                    //else
                                    //{
                                    //    smartSuggestionEngine.readErrorCode(name, 7);
                                    //}
                                }
                                catch (Exception e)
                                {
                                    System.Diagnostics.Debug.WriteLine("An Access violation occured\n" +
                                                      "this seems to suggest the end of the imports section\n");
                                    System.Diagnostics.Debug.WriteLine(e);
                                }
                                pIID++;
                            }
                        }
                    }
                }
            }
            return;
        }

        /// <summary>
        /// return the exported function from the selected file
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="mappedAsImage"></param>
        /// <returns></returns>
        private void LoadExports(string filePath, bool mappedAsImage, List<FunctionObject> ExportedFunctions)
        {
            var hLib = LoadLibraryEx(filePath, 0,
                               DONT_RESOLVE_DLL_REFERENCES | LOAD_IGNORE_CODE_AUTHZ_LEVEL);
            unsafe
            {
                void* hMod = (void*)hLib;
                uint BaseAddress = (uint)hMod;
                if (hMod != null)
                {
                    uint size;
                    IMAGE_EXPORT_DIRECTORY* pExportDir = (IMAGE_EXPORT_DIRECTORY*)Interop.ImageDirectoryEntryToData((void*)hLib, true, Interop.IMAGE_DIRECTORY_ENTRY_EXPORT, out size);
                    if (pExportDir != null)
                    {
                        uint* pFuncNames = (uint*)(BaseAddress + pExportDir->AddressOfNames);
                        for (uint i = 0; i < pExportDir->NumberOfNames; i++)
                        {
                            uint funcNameRva = pFuncNames[i];
                            if (funcNameRva != 0)
                            {
                                char* funcName = (char*)(BaseAddress + funcNameRva);
                                var name = Marshal.PtrToStringAnsi((IntPtr)funcName);
                                ExportedFunctions.Add(new FunctionObject(name));
                            }
                        }
                    }
                }
                //else
                //{
                //    smartSuggestionEngine.readErrorCode("basic error 3", 8);
                //}
            }
            return;
        }

        /// <summary>
        /// return the file path of a given file
        /// input is file name and the current directory name
        /// </summary>
        /// <param name="moduleName"></param>
        /// <param name="currentDirectory"></param>
        /// <returns></returns>
     

        private List<String> GetModulePath(String moduleName, String currentDirectory, String FilePath, PeHeaderReader reader)
        {
            List<string> files = new List<string>();
            // iteratedDirectoryPath.Clear();
            // 0. Look in well-known dlls list

            // 1. Look in application folder
            string applicationFolder = Path.GetDirectoryName(FilePath);
            files.AddRange(GetFiles(applicationFolder, moduleName));

            Environment.SpecialFolder WindowsSystemFolder;

            // try 32 - bit,64 - bit
            if (Is32bitFile(reader))
            {
                WindowsSystemFolder = Environment.SpecialFolder.SystemX86;
            }
            else
            {
                WindowsSystemFolder = Environment.SpecialFolder.System;
            }
          
            string WindowsSystemFolderPath = Environment.GetFolderPath(WindowsSystemFolder);
            files.AddRange(GetFiles(WindowsSystemFolderPath, moduleName));

           

            //try windows folder
            WindowsSystemFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.Windows);
            files.AddRange(GetFiles(WindowsSystemFolderPath, moduleName));

            //try the folders inside PATH Environmental variable
            string PATH = Environment.GetEnvironmentVariable("PATH");
            List<String> PATHFolders = new List<string>(PATH.Split(';'));
            foreach (String SystePath in PATHFolders)
            {
                if (SystePath != "" && !SystePath.Contains(WindowsSystemFolderPath))
                {
                    files.AddRange(GetFiles(SystePath, moduleName));

                }
            }

            //check in current directory
            files.AddRange(GetFiles(currentDirectory, moduleName));


            return files;
        }

        /// <summary>
        /// take input directoy path and the file name and return the file path if file exist in the given directory path
        /// </summary>
        /// <param name="path"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        private List<string> GetFiles(string path, string pattern)
        {
            List<string> files = new List<string>();
            try
            {
                try
                {
                    files.AddRange(Directory.GetFiles(path, pattern, SearchOption.TopDirectoryOnly));
                }
                catch (DirectoryNotFoundException)
                {

                }
                if (files.Count >= 1)
                {
                    return files;
                }
            }
            catch (UnauthorizedAccessException) {
                
            }
            return files;
        }

        /// <summary>
        /// check whether the file is 32 bit or not
        /// </summary>
        /// <returns></returns>
        public bool Is32bitFile(PeHeaderReader reader)
        {
            if (GetMachineType(reader) == "Intel 386")
            {
                return true;
            }
       //     smartSuggestionEngine.readErrorCode(Marshal.GetLastWin32Error());
            return false;
        }

        /// <summary>
        /// return the machine type
        /// </summary>
        /// <returns></returns>
        public string GetMachineType(PeHeaderReader reader)
        {
            IMAGE_FILE_HEADER fileHeader = reader.FileHeader;
            UInt16 machine = fileHeader.Machine;
            //string hexValue = machine.ToString("X");
            switch (machine)
            {
                case 332:
                    return "Intel 386";
                    break;
                case 512:
                    return "Intel 64";
                    break;
                case 34404:
                    return "AMD 64";
                    break;
                default:
                    return "Machine type Unknown";
                    break;
            }
        }

        /// <summary>
        /// check whether the dll or exe is managed or not
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool IsManagedAssembly(string fileName)
        {
            using (Stream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            using (BinaryReader binaryReader = new BinaryReader(fileStream))
            {
                if (fileStream.Length < 64)
                {
                    return false;
                }

                //PE Header starts @ 0x3C (60). Its a 4 byte header.
                fileStream.Position = 0x3C;
                uint peHeaderPointer = binaryReader.ReadUInt32();
                if (peHeaderPointer == 0)
                {
                    peHeaderPointer = 0x80;
                }

                // Ensure there is at least enough room for the following structures:
                //     24 byte PE Signature & Header
                //     28 byte Standard Fields         (24 bytes for PE32+)
                //     68 byte NT Fields               (88 bytes for PE32+)
                // >= 128 byte Data Dictionary Table
                if (peHeaderPointer > fileStream.Length - 256)
                {
                    return false;
                }

                // Check the PE signature.  Should equal 'PE\0\0'.
                fileStream.Position = peHeaderPointer;
                uint peHeaderSignature = binaryReader.ReadUInt32();
                if (peHeaderSignature != 0x00004550)
                {
                    return false;
                }

                // skip over the PEHeader fields
                fileStream.Position += 20;

                const ushort PE32 = 0x10b;
                const ushort PE32Plus = 0x20b;

                // Read PE magic number from Standard Fields to determine format.
                var peFormat = binaryReader.ReadUInt16();
                if (peFormat != PE32 && peFormat != PE32Plus)
                {
                    return false;
                }

                // Read the 15th Data Dictionary RVA field which contains the CLI header RVA.
                // When this is non-zero then the file contains CLI data otherwise not.
                ushort dataDictionaryStart = (ushort)(peHeaderPointer + (peFormat == PE32 ? 232 : 248));
                fileStream.Position = dataDictionaryStart;

                uint cliHeaderRva = binaryReader.ReadUInt32();
                if (cliHeaderRva == 0)
                {
                    return false;
                }

                return true;
            }
        }

        public static string GetCharacterInformation(UInt32 characterValue)
        {
            int i = 0, j, temp = 0;
            char[] hexadecimalNumber = new char[100];
            char temp1;
            string assemblyInformation = null;
            while (characterValue != 0)
            {

                temp = (int)characterValue % 16;
                if (temp < 10)
                    temp = temp + 48;
                else
                    temp = temp + 55;
                temp1 = Convert.ToChar(temp);
                hexadecimalNumber[i++] = temp1;
                characterValue = characterValue / 16;
            }

            switch (hexadecimalNumber[0])
            {
                case '1':
                    assemblyInformation = assemblyInformation + "Relocation information was stripped from the file. " + Environment.NewLine;
                    break;
                case '2':
                    assemblyInformation = assemblyInformation + "The file is executable" + Environment.NewLine;
                    break;
                case '4':
                    assemblyInformation = assemblyInformation + "PE line numbers were stripped from the file." + Environment.NewLine;
                    break;
                case '8':
                    assemblyInformation = assemblyInformation + "PE symbol table entries were stripped from file." + Environment.NewLine;
                    break;
            }
            switch (hexadecimalNumber[1])
            {
                case '1':
                    assemblyInformation = assemblyInformation + "Aggressively trim the working set.  " + Environment.NewLine;
                    break;
                case '2':
                    assemblyInformation = assemblyInformation + "The application can handle addresses larger than 2 GB." + Environment.NewLine;
                    break;
                case '8':
                    assemblyInformation = assemblyInformation + "The bytes of the word are reversed. " + Environment.NewLine;
                    break;
            }
            switch (hexadecimalNumber[2])
            {
                case '1':
                    assemblyInformation = assemblyInformation + "The computer supports 32-bit words. " + Environment.NewLine;
                    break;
                case '2':
                    assemblyInformation = assemblyInformation + "Debugging information was removed and stored separately in another file." + Environment.NewLine;
                    break;
                case '4':
                    assemblyInformation = assemblyInformation + "If the image is on removable media, copy it to and run it from the swap file." + Environment.NewLine;
                    break;
                case '8':
                    assemblyInformation = assemblyInformation + "If the image is on the network, copy it to and run it from the swap file." + Environment.NewLine;
                    break;
            }
            switch (hexadecimalNumber[3])
            {
                case '1':
                    assemblyInformation = assemblyInformation + "The image is a system file. " + Environment.NewLine;
                    break;
                case '2':
                    assemblyInformation = assemblyInformation + "The image is a DLL file. " + Environment.NewLine;
                    break;
                case '4':
                    assemblyInformation = assemblyInformation + "The file should be run only on a uniprocessor computer." + Environment.NewLine;
                    break;
                case '8':
                    assemblyInformation = assemblyInformation + "The bytes of the word are reversed. This flag is obsolete." + Environment.NewLine;
                    break;
            }
            return assemblyInformation;
        }
    }
}
