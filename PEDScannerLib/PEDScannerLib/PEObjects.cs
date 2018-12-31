using System;

namespace PEDScannerLib.Objects
{
    public class HeaderObject
    {
        public string name;
        public string value;
        public HeaderObject(string headerNames, string value)
        {
            this.name = headerNames;
            this.value = value;
        }
    }

  

    public class DependeciesObject
    {
        public string dependencyName;
        public bool isLoadable;
        public DependeciesObject(string dependencyName, bool isLoaded)
        {
            this.dependencyName = dependencyName;
            this.isLoadable = isLoaded;
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
}
