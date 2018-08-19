using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.IO;
using System.Security;


namespace PEDScannerLib.Core
{

    /// <summary>
    /// Reads in the header information of the Portable Executable format.
    /// Provides information such as the date the assembly was compiled.
    /// </summary>
    /// 

    [StructLayout(LayoutKind.Explicit)]
    public unsafe struct IMAGE_IMPORT_BY_NAME
    {
        [FieldOffset(0)]
        public ushort Hint;
        [FieldOffset(2)]
        public fixed char Name[1];
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct IMAGE_IMPORT_DESCRIPTOR
    {
        #region union
        /// <summary>
        /// CSharp doesnt really support unions, but they can be emulated by a field offset 0
        /// </summary>

        [FieldOffset(0)]
        public uint Characteristics;            // 0 for terminating null import descriptor
        [FieldOffset(0)]
        public uint OriginalFirstThunk;         // RVA to original unbound IAT (PIMAGE_THUNK_DATA)
        #endregion

        [FieldOffset(4)]
        public uint TimeDateStamp;
        [FieldOffset(8)]
        public uint ForwarderChain;
        [FieldOffset(12)]
        public uint Name;
        [FieldOffset(16)]
        public uint FirstThunk;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct THUNK_DATA
    {
        [FieldOffset(0)]
        public uint ForwarderString;      // PBYTE 
        [FieldOffset(0)]
        public uint Function;             // PDWORD
        [FieldOffset(0)]
        public uint Ordinal;
        [FieldOffset(0)]
        public uint AddressOfData;        // PIMAGE_IMPORT_BY_NAME
    }


    [StructLayout(LayoutKind.Explicit)]
    public unsafe struct IMAGE_SECTION_HEADER
    {
        [FieldOffset(0)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public fixed char Name[1];
        // public char[] Name;
        [FieldOffset(8)]
        public UInt32 VirtualSize;
        [FieldOffset(12)]
        public UInt32 VirtualAddress;
        [FieldOffset(16)]
        public UInt32 SizeOfRawData;
        [FieldOffset(20)]
        public UInt32 PointerToRawData;
        [FieldOffset(24)]
        public UInt32 PointerToRelocations;
        [FieldOffset(28)]
        public UInt32 PointerToLinenumbers;
        [FieldOffset(32)]
        public UInt16 NumberOfRelocations;
        [FieldOffset(34)]
        public UInt16 NumberOfLinenumbers;
        [FieldOffset(36)]
        public DataSectionFlags Characteristics;

        //public string Section
        //{
        //    get { return new string(Name[1]); }
        //}
    }

    [Flags]
    public enum DataSectionFlags : uint
    {
        /// <summary>
        /// Reserved for future use.
        /// </summary>
        TypeReg = 0x00000000,
        /// <summary>
        /// Reserved for future use.
        /// </summary>
        TypeDsect = 0x00000001,
        /// <summary>
        /// Reserved for future use.
        /// </summary>
        TypeNoLoad = 0x00000002,
        /// <summary>
        /// Reserved for future use.
        /// </summary>
        TypeGroup = 0x00000004,
        /// <summary>
        /// The section should not be padded to the next boundary. This flag is obsolete and is replaced by IMAGE_SCN_ALIGN_1BYTES. This is valid only for object files.
        /// </summary>
        TypeNoPadded = 0x00000008,
        /// <summary>
        /// Reserved for future use.
        /// </summary>
        TypeCopy = 0x00000010,
        /// <summary>
        /// The section contains executable code.
        /// </summary>
        ContentCode = 0x00000020,
        /// <summary>
        /// The section contains initialized data.
        /// </summary>
        ContentInitializedData = 0x00000040,
        /// <summary>
        /// The section contains uninitialized data.
        /// </summary>
        ContentUninitializedData = 0x00000080,
        /// <summary>
        /// Reserved for future use.
        /// </summary>
        LinkOther = 0x00000100,
        /// <summary>
        /// The section contains comments or other information. The .drectve section has this type. This is valid for object files only.
        /// </summary>
        LinkInfo = 0x00000200,
        /// <summary>
        /// Reserved for future use.
        /// </summary>
        TypeOver = 0x00000400,
        /// <summary>
        /// The section will not become part of the image. This is valid only for object files.
        /// </summary>
        LinkRemove = 0x00000800,
        /// <summary>
        /// The section contains COMDAT data. For more information, see section 5.5.6, COMDAT Sections (Object Only). This is valid only for object files.
        /// </summary>
        LinkComDat = 0x00001000,
        /// <summary>
        /// Reset speculative exceptions handling bits in the TLB entries for this section.
        /// </summary>
        NoDeferSpecExceptions = 0x00004000,
        /// <summary>
        /// The section contains data referenced through the global pointer (GP).
        /// </summary>
        RelativeGP = 0x00008000,
        /// <summary>
        /// Reserved for future use.
        /// </summary>
        MemPurgeable = 0x00020000,
        /// <summary>
        /// Reserved for future use.
        /// </summary>
        Memory16Bit = 0x00020000,
        /// <summary>
        /// Reserved for future use.
        /// </summary>
        MemoryLocked = 0x00040000,
        /// <summary>
        /// Reserved for future use.
        /// </summary>
        MemoryPreload = 0x00080000,
        /// <summary>
        /// Align data on a 1-byte boundary. Valid only for object files.
        /// </summary>
        Align1Bytes = 0x00100000,
        /// <summary>
        /// Align data on a 2-byte boundary. Valid only for object files.
        /// </summary>
        Align2Bytes = 0x00200000,
        /// <summary>
        /// Align data on a 4-byte boundary. Valid only for object files.
        /// </summary>
        Align4Bytes = 0x00300000,
        /// <summary>
        /// Align data on an 8-byte boundary. Valid only for object files.
        /// </summary>
        Align8Bytes = 0x00400000,
        /// <summary>
        /// Align data on a 16-byte boundary. Valid only for object files.
        /// </summary>
        Align16Bytes = 0x00500000,
        /// <summary>
        /// Align data on a 32-byte boundary. Valid only for object files.
        /// </summary>
        Align32Bytes = 0x00600000,
        /// <summary>
        /// Align data on a 64-byte boundary. Valid only for object files.
        /// </summary>
        Align64Bytes = 0x00700000,
        /// <summary>
        /// Align data on a 128-byte boundary. Valid only for object files.
        /// </summary>
        Align128Bytes = 0x00800000,
        /// <summary>
        /// Align data on a 256-byte boundary. Valid only for object files.
        /// </summary>
        Align256Bytes = 0x00900000,
        /// <summary>
        /// Align data on a 512-byte boundary. Valid only for object files.
        /// </summary>
        Align512Bytes = 0x00A00000,
        /// <summary>
        /// Align data on a 1024-byte boundary. Valid only for object files.
        /// </summary>
        Align1024Bytes = 0x00B00000,
        /// <summary>
        /// Align data on a 2048-byte boundary. Valid only for object files.
        /// </summary>
        Align2048Bytes = 0x00C00000,
        /// <summary>
        /// Align data on a 4096-byte boundary. Valid only for object files.
        /// </summary>
        Align4096Bytes = 0x00D00000,
        /// <summary>
        /// Align data on an 8192-byte boundary. Valid only for object files.
        /// </summary>
        Align8192Bytes = 0x00E00000,
        /// <summary>
        /// The section contains extended relocations.
        /// </summary>
        LinkExtendedRelocationOverflow = 0x01000000,
        /// <summary>
        /// The section can be discarded as needed.
        /// </summary>
        MemoryDiscardable = 0x02000000,
        /// <summary>
        /// The section cannot be cached.
        /// </summary>
        MemoryNotCached = 0x04000000,
        /// <summary>
        /// The section is not pageable.
        /// </summary>
        MemoryNotPaged = 0x08000000,
        /// <summary>
        /// The section can be shared in memory.
        /// </summary>
        MemoryShared = 0x10000000,
        /// <summary>
        /// The section can be executed as code.
        /// </summary>
        MemoryExecute = 0x20000000,
        /// <summary>
        /// The section can be read.
        /// </summary>
        MemoryRead = 0x40000000,
        /// <summary>
        /// The section can be written to.
        /// </summary>
        MemoryWrite = 0x80000000
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct IMAGE_EXPORT_DIRECTORY
    {
        public UInt32 Characteristics;
        public UInt32 TimeDateStamp;
        public UInt16 MajorVersion;
        public UInt16 MinorVersion;
        public UInt32 Name;
        public UInt32 Base;
        public UInt32 NumberOfFunctions;
        public UInt32 NumberOfNames;
        public UInt32 AddressOfFunctions;     // RVA from base of image
        public UInt32 AddressOfNames;     // RVA from base of image
        public UInt32 AddressOfNameOrdinals;  // RVA from base of image
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct LOADED_IMAGE
    {
        public IntPtr moduleName;
        public IntPtr hFile;
        public IntPtr MappedAddress;
        public IntPtr FileHeader;
        public IntPtr lastRvaSection;
        public UInt32 numbOfSections;
        public IntPtr firstRvaSection;
        public UInt32 charachteristics;
        public ushort systemImage;
        public ushort dosImage;
        public ushort readOnly;
        public ushort version;
        public IntPtr links_1;  // these two comprise the LIST_ENTRY
        public IntPtr links_2;
        public UInt32 sizeOfImage;
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct IMAGE_FILE_HEADER
    {
        public UInt16 Machine;
        public UInt16 NumberOfSections;
        public UInt32 TimeDateStamp;
        public UInt32 PointerToSymbolTable;
        public UInt32 NumberOfSymbols;
        public UInt16 SizeOfOptionalHeader;
        public UInt16 Characteristics;
    }

    public unsafe class Interop
    {
        #region Public Constants
        public static readonly ushort IMAGE_DIRECTORY_ENTRY_IMPORT = 1;
        public static readonly ushort IMAGE_DIRECTORY_ENTRY_EXPORT = 0;

        #endregion
        #region Private Constants
        #region CallingConvention CALLING_CONVENTION
        /// <summary>
        ///     Specifies the calling convention.
        /// </summary>
        /// <remarks>
        ///     Specifies <see cref="CallingConvention.Winapi" /> for Windows to 
        ///     indicate that the default should be used.
        /// </remarks>
        private const CallingConvention CALLING_CONVENTION = CallingConvention.Winapi;
        #endregion CallingConvention CALLING_CONVENTION
        #region IMPORT DLL FUNCTIONS
        private const string KERNEL_DLL = "kernel32";
        private const string DBGHELP_DLL = "Dbghelp";
        private const string IMAGEHLP_DLL = "ImageHlp";
        #endregion
        #endregion Private Constants

        [DllImport(KERNEL_DLL, CallingConvention = CALLING_CONVENTION, EntryPoint = "GetModuleHandleA")]
        public static extern void* GetModuleHandleA(/*IN*/ char* lpModuleName);

        [DllImport(KERNEL_DLL, CallingConvention = CALLING_CONVENTION, EntryPoint = "GetModuleHandleW")]
        public static extern void* GetModuleHandleW(/*IN*/ char* lpModuleName);

        [DllImport(KERNEL_DLL, CallingConvention = CALLING_CONVENTION, EntryPoint = "IsBadReadPtr")]
        public static extern bool IsBadReadPtr(void* lpBase, uint ucb);

        [DllImport(DBGHELP_DLL, CallingConvention = CALLING_CONVENTION, EntryPoint = "ImageDirectoryEntryToData")]
        public static extern void* ImageDirectoryEntryToData(void* Base, bool MappedAsImage, ushort DirectoryEntry, out uint Size);


        [DllImport(DBGHELP_DLL, CallingConvention = CALLING_CONVENTION)]
        public static extern IntPtr ImageRvaToVa(
            IntPtr pNtHeaders,
            IntPtr pBase,
            uint rva,
            IntPtr pLastRvaSection);

        [DllImport(DBGHELP_DLL, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr ImageNtHeader(IntPtr pImageBase);

        [DllImport(IMAGEHLP_DLL, CallingConvention = CallingConvention.Winapi)]
        public static extern bool MapAndLoad(string imageName, string dllPath, out LOADED_IMAGE loadedImage, bool dotDll, bool readOnly);

        [DllImport(KERNEL_DLL)]
        public static extern bool SetFilePointerEx(IntPtr hFile, long liDistanceToMove,
 out long lpNewFilePointer, uint dwMoveMethod);

        //[DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        //public static extern uint SetFilePointer([In] Microsoft.Win32.SafeHandles.SafeFileHandle hFile, [In] int lDistanceToMove, [Out] out int lpDistanceToMoveHigh, [In] EMoveMethod dwMoveMethod);
        //public enum EMoveMethod : uint
        //{
        //    Begin = 0,
        //    Current = 1,
        //    End = 2
        //}


    }


    public class PeHeaderReader
    {
        #region File Header Structures

        public struct IMAGE_DOS_HEADER
        {      // DOS .EXE header
            public UInt16 e_magic;              // Magic number
            public UInt16 e_cblp;               // Bytes on last page of file
            public UInt16 e_cp;                 // Pages in file
            public UInt16 e_crlc;               // Relocations
            public UInt16 e_cparhdr;            // Size of header in paragraphs
            public UInt16 e_minalloc;           // Minimum extra paragraphs needed
            public UInt16 e_maxalloc;           // Maximum extra paragraphs needed
            public UInt16 e_ss;                 // Initial (relative) SS value
            public UInt16 e_sp;                 // Initial SP value
            public UInt16 e_csum;               // Checksum
            public UInt16 e_ip;                 // Initial IP value
            public UInt16 e_cs;                 // Initial (relative) CS value
            public UInt16 e_lfarlc;             // File address of relocation table
            public UInt16 e_ovno;               // Overlay number
            public UInt16 e_res_0;              // Reserved words
            public UInt16 e_res_1;              // Reserved words
            public UInt16 e_res_2;              // Reserved words
            public UInt16 e_res_3;              // Reserved words
            public UInt16 e_oemid;              // OEM identifier (for e_oeminfo)
            public UInt16 e_oeminfo;            // OEM information; e_oemid specific
            public UInt16 e_res2_0;             // Reserved words
            public UInt16 e_res2_1;             // Reserved words
            public UInt16 e_res2_2;             // Reserved words
            public UInt16 e_res2_3;             // Reserved words
            public UInt16 e_res2_4;             // Reserved words
            public UInt16 e_res2_5;             // Reserved words
            public UInt16 e_res2_6;             // Reserved words
            public UInt16 e_res2_7;             // Reserved words
            public UInt16 e_res2_8;             // Reserved words
            public UInt16 e_res2_9;             // Reserved words
            public UInt32 e_lfanew;             // File address of new exe header
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct IMAGE_DATA_DIRECTORY
        {
            public UInt32 VirtualAddress;
            public UInt32 Size;
        }
        [StructLayout(LayoutKind.Explicit)]
        public unsafe struct IMAGE_IMPORT_DESCRIPTOR
        {
            #region union
            /// <summary>
            /// CSharp doesnt really support unions, but they can be emulated by a field offset 0
            /// </summary>

            [FieldOffset(0)]
            public uint Characteristics;            // 0 for terminating null import descriptor
            [FieldOffset(0)]
            public uint OriginalFirstThunk;         // RVA to original unbound IAT (PIMAGE_THUNK_DATA)
            #endregion

            [FieldOffset(4)]
            public uint TimeDateStamp;
            [FieldOffset(8)]
            public uint ForwarderChain;
            [FieldOffset(12)]
            public uint Name;
            [FieldOffset(16)]
            public uint FirstThunk;
        }
        //public struct IMAGE_IMPORT_DESCRIPTOR
        //{
        //   // public UInt32 union;
        //    public UInt32 Characteristics;
        //    public UInt32 OriginalFirstThunk;
        //    public UInt32 ends;
        //    public UInt32 TimeDateStamp;
        //    public UInt32 ForwarderChain;
        //    public UInt32 Name1;
        //    public UInt32 FirstThunk;
        //}

        //public struct IMAGE_IMPORT_BY_NAME
        //{
        //    public UInt32 Hint;
        //    public String Name1;
        //}
        [StructLayout(LayoutKind.Explicit)]
        public unsafe struct IMAGE_IMPORT_BY_NAME
        {
            [FieldOffset(0)]
            public ushort Hint;
            [FieldOffset(2)]
            public fixed char Name[1];
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct IMAGE_OPTIONAL_HEADER32
        {
            public UInt16 Magic;
            public Byte MajorLinkerVersion;
            public Byte MinorLinkerVersion;
            public UInt32 SizeOfCode;
            public UInt32 SizeOfInitializedData;
            public UInt32 SizeOfUninitializedData;
            public UInt32 AddressOfEntryPoint;
            public UInt32 BaseOfCode;
            public UInt32 BaseOfData;
            public UInt32 ImageBase;
            public UInt32 SectionAlignment;
            public UInt32 FileAlignment;
            public UInt16 MajorOperatingSystemVersion;
            public UInt16 MinorOperatingSystemVersion;
            public UInt16 MajorImageVersion;
            public UInt16 MinorImageVersion;
            public UInt16 MajorSubsystemVersion;
            public UInt16 MinorSubsystemVersion;
            public UInt32 Win32VersionValue;
            public UInt32 SizeOfImage;
            public UInt32 SizeOfHeaders;
            public UInt32 CheckSum;
            public UInt16 Subsystem;
            public UInt16 DllCharacteristics;
            public UInt32 SizeOfStackReserve;
            public UInt32 SizeOfStackCommit;
            public UInt32 SizeOfHeapReserve;
            public UInt32 SizeOfHeapCommit;
            public UInt32 LoaderFlags;
            public UInt32 NumberOfRvaAndSizes;

            public IMAGE_DATA_DIRECTORY ExportTable;
            public IMAGE_DATA_DIRECTORY ImportTable;
            public IMAGE_DATA_DIRECTORY ResourceTable;
            public IMAGE_DATA_DIRECTORY ExceptionTable;
            public IMAGE_DATA_DIRECTORY CertificateTable;
            public IMAGE_DATA_DIRECTORY BaseRelocationTable;
            public IMAGE_DATA_DIRECTORY Debug;
            public IMAGE_DATA_DIRECTORY Architecture;
            public IMAGE_DATA_DIRECTORY GlobalPtr;
            public IMAGE_DATA_DIRECTORY TLSTable;
            public IMAGE_DATA_DIRECTORY LoadConfigTable;
            public IMAGE_DATA_DIRECTORY BoundImport;
            public IMAGE_DATA_DIRECTORY IAT;
            public IMAGE_DATA_DIRECTORY DelayImportDescriptor;
            public IMAGE_DATA_DIRECTORY CLRRuntimeHeader;
            public IMAGE_DATA_DIRECTORY Reserved;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]

        public struct IMAGE_OPTIONAL_HEADER64
        {
            public UInt16 Magic;
            public Byte MajorLinkerVersion;
            public Byte MinorLinkerVersion;
            public UInt32 SizeOfCode;
            public UInt32 SizeOfInitializedData;
            public UInt32 SizeOfUninitializedData;
            public UInt32 AddressOfEntryPoint;
            public UInt32 BaseOfCode;
            public UInt64 ImageBase;
            public UInt32 SectionAlignment;
            public UInt32 FileAlignment;
            public UInt16 MajorOperatingSystemVersion;
            public UInt16 MinorOperatingSystemVersion;
            public UInt16 MajorImageVersion;
            public UInt16 MinorImageVersion;
            public UInt16 MajorSubsystemVersion;
            public UInt16 MinorSubsystemVersion;
            public UInt32 Win32VersionValue;
            public UInt32 SizeOfImage;
            public UInt32 SizeOfHeaders;
            public UInt32 CheckSum;
            public UInt16 Subsystem;
            public UInt16 DllCharacteristics;
            public UInt64 SizeOfStackReserve;
            public UInt64 SizeOfStackCommit;
            public UInt64 SizeOfHeapReserve;
            public UInt64 SizeOfHeapCommit;
            public UInt32 LoaderFlags;
            public UInt32 NumberOfRvaAndSizes;

            public IMAGE_DATA_DIRECTORY ExportTable;
            public IMAGE_DATA_DIRECTORY ImportTable;
            public IMAGE_DATA_DIRECTORY ResourceTable;
            public IMAGE_DATA_DIRECTORY ExceptionTable;
            public IMAGE_DATA_DIRECTORY CertificateTable;
            public IMAGE_DATA_DIRECTORY BaseRelocationTable;
            public IMAGE_DATA_DIRECTORY Debug;
            public IMAGE_DATA_DIRECTORY Architecture;
            public IMAGE_DATA_DIRECTORY GlobalPtr;
            public IMAGE_DATA_DIRECTORY TLSTable;
            public IMAGE_DATA_DIRECTORY LoadConfigTable;
            public IMAGE_DATA_DIRECTORY BoundImport;
            public IMAGE_DATA_DIRECTORY IAT;
            public IMAGE_DATA_DIRECTORY DelayImportDescriptor;
            public IMAGE_DATA_DIRECTORY CLRRuntimeHeader;
            public IMAGE_DATA_DIRECTORY Reserved;
        }

        //public struct IMAGE_NT_HEADERS32
        //{
        //    public uint Signature;
        //    IMAGE_FILE_HEADER FileHeader;
        //    IMAGE_OPTIONAL_HEADER32 OptionalHeader;
        //}

        //public struct IMAGE_NT_HEADERS64
        //{
        //    public uint Signature;
        //    public IMAGE_FILE_HEADER FileHeader;
        //    public IMAGE_OPTIONAL_HEADER64 OptionalHeader;
        //}

        [StructLayout(LayoutKind.Explicit)]
        public struct THUNK_DATA
        {
            [FieldOffset(0)]
            public uint ForwarderString;      // PBYTE 
            [FieldOffset(0)]
            public uint Function;             // PDWORD
            [FieldOffset(0)]
            public uint Ordinal;
            [FieldOffset(0)]
            public uint AddressOfData;        // PIMAGE_IMPORT_BY_NAME
        }



        // Grabbed the following 2 definitions from http://www.pinvoke.net/default.aspx/Structures/IMAGE_SECTION_HEADER.html

        [StructLayout(LayoutKind.Explicit)]
        public struct IMAGE_SECTION_HEADER
        {
            [FieldOffset(0)]
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public char[] Name;
            [FieldOffset(8)]
            public UInt32 VirtualSize;
            [FieldOffset(12)]
            public UInt32 VirtualAddress;
            [FieldOffset(16)]
            public UInt32 SizeOfRawData;
            [FieldOffset(20)]
            public UInt32 PointerToRawData;
            [FieldOffset(24)]
            public UInt32 PointerToRelocations;
            [FieldOffset(28)]
            public UInt32 PointerToLinenumbers;
            [FieldOffset(32)]
            public UInt16 NumberOfRelocations;
            [FieldOffset(34)]
            public UInt16 NumberOfLinenumbers;
            [FieldOffset(36)]
            public DataSectionFlags Characteristics;

            public string Section
            {
                get { return new string(Name); }
            }
        }

        [Flags]
        public enum DataSectionFlags : uint
        {
            /// <summary>
            /// Reserved for future use.
            /// </summary>
            TypeReg = 0x00000000,
            /// <summary>
            /// Reserved for future use.
            /// </summary>
            TypeDsect = 0x00000001,
            /// <summary>
            /// Reserved for future use.
            /// </summary>
            TypeNoLoad = 0x00000002,
            /// <summary>
            /// Reserved for future use.
            /// </summary>
            TypeGroup = 0x00000004,
            /// <summary>
            /// The section should not be padded to the next boundary. This flag is obsolete and is replaced by IMAGE_SCN_ALIGN_1BYTES. This is valid only for object files.
            /// </summary>
            TypeNoPadded = 0x00000008,
            /// <summary>
            /// Reserved for future use.
            /// </summary>
            TypeCopy = 0x00000010,
            /// <summary>
            /// The section contains executable code.
            /// </summary>
            ContentCode = 0x00000020,
            /// <summary>
            /// The section contains initialized data.
            /// </summary>
            ContentInitializedData = 0x00000040,
            /// <summary>
            /// The section contains uninitialized data.
            /// </summary>
            ContentUninitializedData = 0x00000080,
            /// <summary>
            /// Reserved for future use.
            /// </summary>
            LinkOther = 0x00000100,
            /// <summary>
            /// The section contains comments or other information. The .drectve section has this type. This is valid for object files only.
            /// </summary>
            LinkInfo = 0x00000200,
            /// <summary>
            /// Reserved for future use.
            /// </summary>
            TypeOver = 0x00000400,
            /// <summary>
            /// The section will not become part of the image. This is valid only for object files.
            /// </summary>
            LinkRemove = 0x00000800,
            /// <summary>
            /// The section contains COMDAT data. For more information, see section 5.5.6, COMDAT Sections (Object Only). This is valid only for object files.
            /// </summary>
            LinkComDat = 0x00001000,
            /// <summary>
            /// Reset speculative exceptions handling bits in the TLB entries for this section.
            /// </summary>
            NoDeferSpecExceptions = 0x00004000,
            /// <summary>
            /// The section contains data referenced through the global pointer (GP).
            /// </summary>
            RelativeGP = 0x00008000,
            /// <summary>
            /// Reserved for future use.
            /// </summary>
            MemPurgeable = 0x00020000,
            /// <summary>
            /// Reserved for future use.
            /// </summary>
            Memory16Bit = 0x00020000,
            /// <summary>
            /// Reserved for future use.
            /// </summary>
            MemoryLocked = 0x00040000,
            /// <summary>
            /// Reserved for future use.
            /// </summary>
            MemoryPreload = 0x00080000,
            /// <summary>
            /// Align data on a 1-byte boundary. Valid only for object files.
            /// </summary>
            Align1Bytes = 0x00100000,
            /// <summary>
            /// Align data on a 2-byte boundary. Valid only for object files.
            /// </summary>
            Align2Bytes = 0x00200000,
            /// <summary>
            /// Align data on a 4-byte boundary. Valid only for object files.
            /// </summary>
            Align4Bytes = 0x00300000,
            /// <summary>
            /// Align data on an 8-byte boundary. Valid only for object files.
            /// </summary>
            Align8Bytes = 0x00400000,
            /// <summary>
            /// Align data on a 16-byte boundary. Valid only for object files.
            /// </summary>
            Align16Bytes = 0x00500000,
            /// <summary>
            /// Align data on a 32-byte boundary. Valid only for object files.
            /// </summary>
            Align32Bytes = 0x00600000,
            /// <summary>
            /// Align data on a 64-byte boundary. Valid only for object files.
            /// </summary>
            Align64Bytes = 0x00700000,
            /// <summary>
            /// Align data on a 128-byte boundary. Valid only for object files.
            /// </summary>
            Align128Bytes = 0x00800000,
            /// <summary>
            /// Align data on a 256-byte boundary. Valid only for object files.
            /// </summary>
            Align256Bytes = 0x00900000,
            /// <summary>
            /// Align data on a 512-byte boundary. Valid only for object files.
            /// </summary>
            Align512Bytes = 0x00A00000,
            /// <summary>
            /// Align data on a 1024-byte boundary. Valid only for object files.
            /// </summary>
            Align1024Bytes = 0x00B00000,
            /// <summary>
            /// Align data on a 2048-byte boundary. Valid only for object files.
            /// </summary>
            Align2048Bytes = 0x00C00000,
            /// <summary>
            /// Align data on a 4096-byte boundary. Valid only for object files.
            /// </summary>
            Align4096Bytes = 0x00D00000,
            /// <summary>
            /// Align data on an 8192-byte boundary. Valid only for object files.
            /// </summary>
            Align8192Bytes = 0x00E00000,
            /// <summary>
            /// The section contains extended relocations.
            /// </summary>
            LinkExtendedRelocationOverflow = 0x01000000,
            /// <summary>
            /// The section can be discarded as needed.
            /// </summary>
            MemoryDiscardable = 0x02000000,
            /// <summary>
            /// The section cannot be cached.
            /// </summary>
            MemoryNotCached = 0x04000000,
            /// <summary>
            /// The section is not pageable.
            /// </summary>
            MemoryNotPaged = 0x08000000,
            /// <summary>
            /// The section can be shared in memory.
            /// </summary>
            MemoryShared = 0x10000000,
            /// <summary>
            /// The section can be executed as code.
            /// </summary>
            MemoryExecute = 0x20000000,
            /// <summary>
            /// The section can be read.
            /// </summary>
            MemoryRead = 0x40000000,
            /// <summary>
            /// The section can be written to.
            /// </summary>
            MemoryWrite = 0x80000000
        }

        #endregion File Header Structures

        #region Private Fields

        /// <summary>
        /// The DOS header
        /// </summary>
        private IMAGE_DOS_HEADER dosHeader;
        /// <summary>
        /// The file header
        /// </summary>
        private IMAGE_FILE_HEADER fileHeader;

        /// <summary>
        /// Optional 32 bit file header 
        /// </summary>
        private IMAGE_OPTIONAL_HEADER32 optionalHeader32;
        /// <summary>
        /// Optional 64 bit file header 
        /// </summary>
        private IMAGE_OPTIONAL_HEADER64 optionalHeader64;
        /// <summary>
        /// Image Section headers. Number of sections is in the file header.
        /// </summary>
        private IMAGE_SECTION_HEADER[] imageSectionHeaders;

        #endregion Private Fields

        #region Public Methods

        public PeHeaderReader(string filePath)
        {
            if (filePath != null)
            {
                // Read in the DLL or EXE and get the timestamp
                using (FileStream stream = new FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                {
                    BinaryReader reader = new BinaryReader(stream);
                    dosHeader = FromBinaryReader<IMAGE_DOS_HEADER>(reader);

                    // Add 4 bytes to the offset
                    stream.Seek(dosHeader.e_lfanew, SeekOrigin.Begin);

                    UInt32 ntHeadersSignature = reader.ReadUInt32();
                    fileHeader = FromBinaryReader<IMAGE_FILE_HEADER>(reader);
                    if (this.Is32BitHeader)
                    {
                        optionalHeader32 = FromBinaryReader<IMAGE_OPTIONAL_HEADER32>(reader);
                    }
                    else
                    {
                        optionalHeader64 = FromBinaryReader<IMAGE_OPTIONAL_HEADER64>(reader);
                    }
                    // imageNTHeader64 = FromBinaryReader<PeHeaderReader.IMAGE_NT_HEADERS64>(reader);
                    imageSectionHeaders = new IMAGE_SECTION_HEADER[fileHeader.NumberOfSections];
                    for (int headerNo = 0; headerNo < imageSectionHeaders.Length; ++headerNo)
                    {
                        imageSectionHeaders[headerNo] = FromBinaryReader<IMAGE_SECTION_HEADER>(reader);
                    }

                }
            }
        }



        //public static PeHeaderReader GetCallingAssemblyHeader()
        //{
        //    // Get the path to the calling assembly, which is the path to the
        //    // DLL or EXE that we want the time of
        //    string filePath = System.Reflection.Assembly.GetCallingAssembly().Location;

        //    // Get and return the timestamp
        //    return new PeHeaderReader(filePath);
        //}

        /// <summary>
        /// Gets the header of the .NET assembly that called this function
        /// </summary>
        /// <returns></returns>
        //public static PeHeaderReader GetAssemblyHeader()
        //{
        //    // Get the path to the calling assembly, which is the path to the
        //    // DLL or EXE that we want the time of
        //    string filePath = System.Reflection.Assembly.GetAssembly(typeof(PeHeaderReader)).Location;

        //    // Get and return the timestamp
        //    return new PeHeaderReader(filePath);
        //}

        /// <summary>
        /// Reads in a block from a file and converts it to the struct
        /// type specified by the template parameter
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static T FromBinaryReader<T>(BinaryReader reader)
        {
            // Read in a byte array
            byte[] bytes = reader.ReadBytes(Marshal.SizeOf(typeof(T)));

            // Pin the managed memory while, copy it out the data, then unpin it
            GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            T theStructure = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            handle.Free();

            return theStructure;
        }

        #endregion Public Methods

        #region Properties

        /// <summary>
        /// Gets if the file header is 32 bit or not
        /// </summary>
        public bool Is32BitHeader
        {
            get
            {
                UInt16 IMAGE_FILE_32BIT_MACHINE = 0x0100;
                return (IMAGE_FILE_32BIT_MACHINE & FileHeader.Characteristics) == IMAGE_FILE_32BIT_MACHINE;
            }
        }

        /// <summary>
        /// Gets the file header
        /// </summary>
        public IMAGE_FILE_HEADER FileHeader
        {
            get
            {
                return fileHeader;
            }
        }

        /// <summary>
        /// Gets the optional header
        /// </summary>
        public IMAGE_OPTIONAL_HEADER32 OptionalHeader32
        {
            get
            {
                return optionalHeader32;
            }
        }

        /// <summary>
        /// Gets the optional header
        /// </summary>
        public IMAGE_OPTIONAL_HEADER64 OptionalHeader64
        {
            get
            {
                return optionalHeader64;
            }
        }

        //public PeHeaderReader.IMAGE_NT_HEADERS64 ImageNTHeaders64
        //{
        //    get
        //    {
        //        return imageNTHeader64;
        //    }
        //}

        public IMAGE_SECTION_HEADER[] ImageSectionHeaders
        {
            get
            {
                return imageSectionHeaders;
            }
        }

        /// <summary>
        /// Gets the timestamp from the file header
        /// </summary>
        //public DateTime TimeStamp
        //{
        //    get
        //    {
        //        // Timestamp is a date offset from 1970
        //        DateTime returnValue = new DateTime(1970, 1, 1, 0, 0, 0);

        //        // Add in the number of seconds since 1970/1/1
        //        returnValue = returnValue.AddSeconds(fileHeader.TimeDateStamp);
        //        // Adjust to local timezone
        //        returnValue += TimeZone.CurrentTimeZone.GetUtcOffset(returnValue);

        //        return returnValue;
        //    }
        //}

        #endregion Properties
    }
    public class HeaderObject
    {
        public string name;
        public UInt32 value;
        public HeaderObject(string headerNames, UInt32 value)
        {
            this.name = headerNames;
            this.value = value;
        }
    }

    public class FunctionObject
    {
        public string function;
        public FunctionObject(string functionName)
        {
            this.function = functionName;
        }
    }

    public class ImportFunctionObject : FunctionObject
    {
        public UInt32 baseAddress;
        public string dependency;
        public ImportFunctionObject(string functionName, UInt32 baseAddress, string dependency) : base(functionName)
        {
            this.baseAddress = baseAddress;
            this.dependency = dependency;
        }
    }

    public class SectionObject
    {
        public string name;
        public UInt32 virtualAddress;
        public UInt32 virtualsize;
        public UInt32 rawDataOffset;
        public UInt32 rawDataSize;

        public SectionObject(string name, UInt32 virtualAddress, UInt32 virtualsize, UInt32 rawDataOffset, UInt32 rawDataSize)
        {
            this.name = name;
            this.virtualAddress = virtualAddress;
            this.virtualsize = virtualsize;
            this.rawDataOffset = rawDataOffset;
            this.rawDataSize = rawDataSize;
        }

    }

    public class DirectoryObject
    {
        public string name;
        public UInt32 RVA;
        public UInt32 Size;
        public DirectoryObject(string name, UInt32 RVA, UInt32 Size)
        {
            this.name = name;
            this.RVA = RVA;
            this.Size = Size;
        }
    }

    public class PortableExecutable
    {
        const uint DONT_RESOLVE_DLL_REFERENCES = 0x00000001;
        const uint LOAD_IGNORE_CODE_AUTHZ_LEVEL = 0x00000010;

        [DllImport("kernel32.dll"), SuppressUnmanagedCodeSecurity]
        static extern uint LoadLibraryEx(string fileName, uint notUsedMustBeZero, uint flags);

        public string Name;
        public string FilePath;
        public PeHeaderReader reader;

        public List<PortableExecutable> Dependencies;
        public List<FunctionObject> ExportedFunctions;
        public List<ImportFunctionObject> ImportFunctions;
        public List<HeaderObject> Headers;
        public List<SectionObject> Sections;
        public List<DirectoryObject> Directories;
        public List<string> ImportNames;
        // UInt16 SafeDllSearchMode =1;
        public string directoryPath = Directory.GetCurrentDirectory();
        public PortableExecutable(string Name, string FilePath)
        {
            this.Name = Name;
            this.FilePath = FilePath;
            reader = new PeHeaderReader(FilePath);


            ExportedFunctions = new List<FunctionObject>();
            ImportFunctions = new List<ImportFunctionObject>();
            Headers = new List<HeaderObject>();
            Sections = new List<SectionObject>();
            Directories = new List<DirectoryObject>();
            ImportNames = new List<string>();
            Dependencies = new List<PortableExecutable>();
            LoadImports(FilePath, true);
            LoadExports(FilePath, true);
            GetHeader();
            // MakeDependencies();
            GetDirectories();
        }




        public List<PortableExecutable> MakeDependencies()
        {
            PortableExecutable PE;
            foreach (string name in ImportNames)
            {
                string filePath = GetModulePath(name, directoryPath);
                PE = new PortableExecutable(name, filePath);
                Dependencies.Add(PE);
            }
            return Dependencies;
        }

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
                    // PeHeaderReader.IMAGE_DATA_DIRECTORY ImportAddressTable = header32.BoundImport;
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
                                    // System.Diagnostics.Debug.WriteLine("pIID->Name = {0} BaseAddress - {1}", name, (uint)BaseAddress);
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
                                                string sImportName = Marshal.PtrToStringAnsi((IntPtr)szImportName); // yes i know i am a lazy ass
                                                                                                                    // System.Diagnostics.Debug.WriteLine("imports ({0}).{1}@{2} - Address: {3}", name, sImportName, Ord, pThunkOrg->Function);
                                                UInt32 Address = pThunkOrg->Function;
                                                ImportFunctions.Add(new ImportFunctionObject(sImportName, Address, name));
                                                //TODO add it as a dependecny as well
                                            }
                                            else
                                            {
                                                System.Diagnostics.Debug.WriteLine("Bad ReadPtr Detected or EOF on Imports");
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

        public List<FunctionObject> LoadExports(string filePath, bool mappedAsImage)
        {
            var hLib = LoadLibraryEx(filePath, 0,
                               DONT_RESOLVE_DLL_REFERENCES | LOAD_IGNORE_CODE_AUTHZ_LEVEL);
            unsafe
            {

                void* hMod = (void*)hLib;
                uint BaseAddress = (uint)hMod;

                // var hMod = (void*)loadedImage.MappedAddress;

                if (hMod != null)
                {

                    uint size;
                    IMAGE_EXPORT_DIRECTORY* pExportDir = (IMAGE_EXPORT_DIRECTORY*)Interop.ImageDirectoryEntryToData((void*)hLib, true, Interop.IMAGE_DIRECTORY_ENTRY_EXPORT, out size);

                    if (pExportDir != null)
                    {
                        // System.Diagnostics.Debug.WriteLine("Got Image Export Descriptor");

                        uint* pFuncNames = (uint*)(BaseAddress + pExportDir->AddressOfNames);

                        for (uint i = 0; i < pExportDir->NumberOfNames; i++)
                        {

                            uint funcNameRva = pFuncNames[i];
                            if (funcNameRva != 0)
                            {

                                char* funcName = (char*)(BaseAddress + funcNameRva);
                                var name = Marshal.PtrToStringAnsi((IntPtr)funcName);
                                ExportedFunctions.Add(new FunctionObject(name));
                                // System.Diagnostics.Debug.WriteLine("Exported functionName: {0}", name);
                                // exports.Add(name);
                            }

                        }

                    }

                }

            }
            return ExportedFunctions;
        }

        public String GetModulePath(String moduleName, String currentDirectory)
        {
            // 0. Look in well-known dlls list

            // 1. Look in application folder
            string applicationFolder = Path.GetDirectoryName(FilePath);
            List<string> files = GetFiles(applicationFolder, moduleName);

            if (files.Count > 0)
            {
                return files.First();
            }

            //try 32-bit,64-bit 
            Environment.SpecialFolder WindowsSystemFolder;
            if (Is32bitFile())
            {
                WindowsSystemFolder = Environment.SpecialFolder.System;
            }
            else
            {
                WindowsSystemFolder = Environment.SpecialFolder.SystemX86;
            }
            String WindowsSystemFolderPath = Environment.GetFolderPath(WindowsSystemFolder);
            //  String WindowsSystemFolderPath = "C:/Windows";

            files = GetFiles(WindowsSystemFolderPath, moduleName);

            if (files.Count > 0)
            {
                return files.First();
            }


            //try 64-bit
            // WindowsSystemFolder = Environment.SpecialFolder.System;
            //WindowsSystemFolderPath = Environment.GetFolderPath(WindowsSystemFolder);
            //files = GetFiles(WindowsSystemFolderPath, moduleName);

            //if (files.Count > 0)
            //{
            //    return files.First();
            //}


            //try windows folder
            WindowsSystemFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.Windows);
            files = GetFiles(WindowsSystemFolderPath, moduleName);

            if (files.Count > 0)
            {
                return files.First();
            }


            //check system PATH
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

                foreach (var directory in Directory.GetDirectories(path))
                    files.AddRange(GetFiles(directory, pattern));
            }
            catch (UnauthorizedAccessException) { }

            return files;
        }

        public bool Is32bitFile()
        {
            if (GetMachineType() == "Intel 386")
            {
                return true;
            }

            return false;
        }

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


    }


}
