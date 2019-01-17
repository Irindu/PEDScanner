using System;

namespace PEDScannerLib.Objects
{
    public class HeaderObject
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public HeaderObject(string headerNames, string value)
        {
            this.Name = headerNames;
            this.Value = value;
        }
    }
    public class ErrorObject
    {
        public string DependencyName { get; set; }
        public string Error { get; set; }
        public ErrorObject(string dependencyName, string error)
        {
            this.DependencyName = dependencyName;
            this.Error = error;
        }
    }

    //public class FunctionObject
    //{
    //    public string Function { get; set; }

    //    public FunctionObject(string functionName)
    //    {
    //        this.Function = functionName;
    //    }
    //}

    //public class ImportFunctionObject : FunctionObject
    //{
    //    public UInt32 BaseAddress { get; set; }
    //    public string Dependency { get; set; }

    //    public ImportFunctionObject(string functionName, UInt32 baseAddress, string dependency) : base(functionName)
    //    {
    //        this.BaseAddress = baseAddress;
    //        this.Dependency = dependency;
    //    }
    //}

    public class DependeciesObject
    {
        public string DependencyName { get; set; }
        public bool IsLoadable { get; set; }

        public DependeciesObject(string dependencyName, bool isLoaded)
        {
            this.DependencyName = dependencyName;
            this.IsLoadable = isLoaded;
        }
    }
    public class SectionObject
    {
        public string Name { get; set; }
        public UInt32 VirtualAddress { get; set; }
        public UInt32 VirtualSize { get; set; }
        public UInt32 RawDataOffset { get; set; }
        public UInt32 RawDataSize { get; set; }

        public SectionObject(string name, UInt32 virtualAddress, UInt32 virtualsize, UInt32 rawDataOffset, UInt32 rawDataSize)
        {
            this.Name = name;
            this.VirtualAddress = virtualAddress;
            this.VirtualSize = virtualsize;
            this.RawDataOffset = rawDataOffset;
            this.RawDataSize = rawDataSize;
        }

    }

    public class DirectoryObject
    {
        public string Name { get; set; }
        public UInt32 RVA { get; set; }
        public UInt32 Size { get; set; }
        public DirectoryObject(string name, UInt32 RVA, UInt32 Size)
        {
            this.Name = name;
            this.RVA = RVA;
            this.Size = Size;
        }
    }
}
