using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.IO;
using System.Security;
using System.Reflection;
using PEDScannerLib.Struct;
using PEDScannerLib.Objects;


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

        public string Name;
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
        public Assembly assembly;
        public string directoryPath = Directory.GetCurrentDirectory();
        public PortableExecutable(string Name, string FilePath, bool IsLoadable)
        {
            this.Name = Name;
            this.FilePath = FilePath;
            this.IsLoadable = IsLoadable;
            reader = new PeHeaderReader(FilePath);

            ExportedFunctions = new List<FunctionObject>();
            ImportFunctions = new List<ImportFunctionObject>();
            Headers = new List<HeaderObject>();
            Sections = new List<SectionObject>();
            Directories = new List<DirectoryObject>();
            ImportNames = new List<string>();
            DependencyNames = new List<DependeciesObject>();
            Dependencies = new List<PortableExecutable>();

            LoadExports(FilePath, true);
            GetHeader();
            LoadImports(FilePath, true);
            GetAssemblyDependencies(FilePath);
            MakeDependencies();
            GetDirectories();
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
        public List<PortableExecutable> MakeDependencies()
        {
            PortableExecutable PE;
            unsafe
            {
                foreach (string name in ImportNames)
                {
                    string filePath = GetModulePath(name, directoryPath);

                    var hLib2 = LoadLibraryEx(filePath, 0,
                                          DONT_RESOLVE_DLL_REFERENCES | LOAD_IGNORE_CODE_AUTHZ_LEVEL);
                    if (!CheckLibrary(name))
                    {
                        PE = new PortableExecutable(name, filePath, true);
                    }
                    else
                    {
                        PE = new PortableExecutable(name, filePath, false);
                    }
                    Dependencies.Add(PE);

                }
                return Dependencies;
            }
        }

        /// <summary>
        /// find the .Net dependencies for managed dlls.
        /// input is file path of the file and output is .Net dependencies as ImportFunctionObject 
        /// </summary>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        public List<ImportFunctionObject> GetAssemblyDependencies(string FilePath)
        {
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

                            ImportFunctions.Add(new ImportFunctionObject(method.Name, 0, attr.Value));
                            ImportNames.Add(attr.Value);
                        }
                    }

                    // Find the set of assemblies our assemblies reference
                    foreach (AssemblyName an in assembly.GetReferencedAssemblies())
                    {
                        ImportNames.Add((an.Name + ".dll"));
                    }
                }
            }

            return ImportFunctions;
        }

        /// <summary>
        /// find the header information
        /// </summary>
        /// <returns></returns>
        public List<HeaderObject> GetHeader()
        {
            IMAGE_FILE_HEADER fileHeader = reader.FileHeader;
            UInt16 machine = fileHeader.Machine;
            UInt16 numberOfSections = fileHeader.NumberOfSections;
            UInt32 timeDateStamp = fileHeader.TimeDateStamp;
            UInt32 pointerToSymbolTable = fileHeader.PointerToSymbolTable;
            UInt32 numberOfSymbols = fileHeader.NumberOfSymbols;
            UInt16 sizeOfOptionalHeader = fileHeader.SizeOfOptionalHeader;
            UInt16 characteristics = fileHeader.Characteristics;
            Headers.Add(new HeaderObject("Machine", machine));
            Headers.Add(new HeaderObject("Number of sections", numberOfSections));
            Headers.Add(new HeaderObject("Timestamp", timeDateStamp));
            Headers.Add(new HeaderObject("Pointer to symbol table", pointerToSymbolTable));
            Headers.Add(new HeaderObject("Number of symbols", numberOfSymbols));
            Headers.Add(new HeaderObject("Size of optional header", sizeOfOptionalHeader));
            Headers.Add(new HeaderObject("Characteristics", characteristics));
            return Headers;
        }

        /// <summary>
        /// find the section information for a given file
        /// </summary>
        /// <returns></returns>
        public List<SectionObject> GetSections()
        {
            PeHeaderReader.IMAGE_SECTION_HEADER[] sections = reader.ImageSectionHeaders;
            IMAGE_FILE_HEADER fileheader = reader.FileHeader;
            UInt32 numberofSection = fileheader.NumberOfSections;

            System.Diagnostics.Debug.WriteLine("number={0}", numberofSection);

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
                //System.Diagnostics.Debug.WriteLine(section.Section);
                Sections.Add(new SectionObject(nameOfSection, virtualAddress, virtualSize, pointerToRawData, sizeOFrawData));
            }
            return Sections;
        }

        /// <summary>
        /// find the directories
        /// </summary>
        /// <returns></returns>
        public List<DirectoryObject> GetDirectories()
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
            return Directories;
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
        public List<ImportFunctionObject> LoadImports(string filePath, bool mappedAsImage)
        {
            var hLib = LoadLibraryEx(filePath, 0,
                               DONT_RESOLVE_DLL_REFERENCES | LOAD_IGNORE_CODE_AUTHZ_LEVEL);
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
                                            // System.Diagnostics.Debug.WriteLine("imports ({0}).Ordinal{1} - Address: {2}", name, Ord, pThunkOrg->Function);
                                        }
                                        else
                                        {
                                            IMAGE_IMPORT_BY_NAME* pIBN = (IMAGE_IMPORT_BY_NAME*)(BaseAddress + pThunkOrg->AddressOfData);

                                            if (!Interop.IsBadReadPtr((void*)pIBN, (uint)sizeof(IMAGE_IMPORT_BY_NAME)))
                                            {
                                                Ord = pIBN->Hint;
                                                szImportName = (char*)pIBN->Name;
                                                string sImportName = Marshal.PtrToStringAnsi((IntPtr)szImportName);
                                                // System.Diagnostics.Debug.WriteLine("imports ({0}).{1}@{2} - Address: {3}", name, sImportName, Ord, pThunkOrg->Function);
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
            return ImportFunctions;
        }

        /// <summary>
        /// return the exported function from the selected file
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="mappedAsImage"></param>
        /// <returns></returns>
        public List<FunctionObject> LoadExports(string filePath, bool mappedAsImage)
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
            }
            return ExportedFunctions;
        }

        /// <summary>
        /// return the file path of a given file
        /// input is file name and the current directory name
        /// </summary>
        /// <param name="moduleName"></param>
        /// <param name="currentDirectory"></param>
        /// <returns></returns>
        public String GetModulePath(String moduleName, String currentDirectory)
        {
            // iteratedDirectoryPath.Clear();
            // 0. Look in well-known dlls list

            // 1. Look in application folder
            string applicationFolder = Path.GetDirectoryName(FilePath);
            List<string> files = GetFiles(applicationFolder, moduleName);
            if (files.Count > 0)
            {
                return files.First();
            }
            Environment.SpecialFolder WindowsSystemFolder;

            //try 32-bit,64-bit 
            if (Is32bitFile())
            {
                WindowsSystemFolder = Environment.SpecialFolder.SystemX86;
            }
            else
            {
                WindowsSystemFolder = Environment.SpecialFolder.System;
            }
            string WindowsSystemFolderPath = Environment.GetFolderPath(WindowsSystemFolder);
            files = GetFiles(WindowsSystemFolderPath, moduleName);
            if (files.Count > 0)
            {
                return files.First();
            }
            //try windows folder
            WindowsSystemFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.Windows);
            files = GetFiles(WindowsSystemFolderPath, moduleName);
            if (files.Count > 0)
            {
                return files.First();
            }
            //try the folders inside PATH Environmental variable
            string PATH = Environment.GetEnvironmentVariable("PATH");
            List<String> PATHFolders = new List<string>(PATH.Split(';'));
            foreach (String SystePath in PATHFolders)
            {
                if (SystePath != "" && !SystePath.Contains(WindowsSystemFolderPath))
                {
                    files = GetFiles(SystePath, moduleName);
                    if (files.Count > 0)
                    {
                        return files.First();
                    }
                }
            }

            //check in current directory
            files = GetFiles(currentDirectory, moduleName);

            if (files.Count > 0)
            {
                return files.First();
            }
            return null;
        }

        /// <summary>
        /// take input directoy path and the file name and return the file path if file exist in the given directory path
        /// </summary>
        /// <param name="path"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public List<string> GetFiles(string path, string pattern)
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
            catch (UnauthorizedAccessException) { }
            return files;
        }

        /// <summary>
        /// check whether the file is 32 bit or not
        /// </summary>
        /// <returns></returns>
        public bool Is32bitFile()
        {
            if (GetMachineType() == "Intel 386")
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// return the machine type
        /// </summary>
        /// <returns></returns>
        public string GetMachineType()
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
    }
}
