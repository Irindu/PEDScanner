using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Description;
using Objects;
using ClassLibraryServer.Struct;
using System.Runtime.InteropServices;

namespace Objects
{

    public class FunctionObject
    {
        public string Function;
        public FunctionObject()
        {
            this.FunctionList = new List<FunctionObject>();
        }
        public FunctionObject(string functionName)
        {
            this.Function = functionName;
        }
        public List<FunctionObject> FunctionList { get; set; }


    }

    public class ImportFunctionObject : FunctionObject
    {

        public UInt64 BaseAddress;
        public string Dependency;
        public ImportFunctionObject()
        {
             this.FunctionObjectList = new List<ImportFunctionObject>();
        }

        public ImportFunctionObject(string functionName, UInt64 baseAddress, string dependency) : base(functionName)
        {
            this.BaseAddress = baseAddress;
            this.Dependency = dependency;
        }
        public List<ImportFunctionObject> FunctionObjectList { get; set; }

    }

    public class ExportObject
    {
        public ExportObject()
        {
            this.ExportFunctionObjectList = new List<FunctionObject>();
        }

        public ExportObject(List<FunctionObject> list)
        {
            this.ExportFunctionObjectList = list;
        }

        public List<FunctionObject> ExportFunctionObjectList { get; set; }
    }
    public class MyObject
    {
        public MyObject()
        {
            this.FunctionObjectList = new List<ImportFunctionObject>();
        }

        public MyObject(List<ImportFunctionObject> list)
        {
            this.FunctionObjectList = list;
        }
        public List<ImportFunctionObject> FunctionObjectList { get; set; }


    }


}

namespace Server64
{
    [ServiceContract()]
    public interface ISimpleService64
    {

        [OperationContract]
        ExportObject Load64Exports(ExportObject myObject, string filePath, bool mappedAsImage);

        [OperationContract]
        MyObject Load64Imports(MyObject myObject, string filePath, bool mappedAsImage);
    }

    public class Service64 : ISimpleService64
    {
      
        [DllImport("kernel32", SetLastError = true)]
        static extern IntPtr LoadLibrary(string lpFileName);

        [DllImport("kernel32.dll")]
        static extern uint GetLastError();

      
        public ExportObject Load64Exports(ExportObject myObject, string filePath, bool mappedAsImage)
        {
           
            List<FunctionObject> exportList = myObject.ExportFunctionObjectList;

            //var hLib = LoadLibraryEx(filePath, 0,
            //                   DONT_RESOLVE_DLL_REFERENCES | LOAD_IGNORE_CODE_AUTHZ_LEVEL);
            var hLib = LoadLibrary(filePath);
            unsafe
            {
                void* hMod = (void*)hLib;
                ulong BaseAddress = (ulong)hMod;
               
                if (hMod != null)
                {
                    ulong size;
                    IMAGE_EXPORT_DIRECTORY* pExportDir = (IMAGE_EXPORT_DIRECTORY*)Interop.ImageDirectoryEntryToData((void*)hLib, true, Interop.IMAGE_DIRECTORY_ENTRY_EXPORT, out size);
                    if (pExportDir != null)
                    {
                        ulong* pFuncNames = (ulong*)(BaseAddress + pExportDir->AddressOfNames);
                        for (uint i = 0; i < pExportDir->NumberOfNames; i++)
                        {
                            ulong funcNameRva = pFuncNames[i];
                           // ulong funcNameRva = pFuncNames[i];
                            if (funcNameRva != 0)
                            {
                                char* funcName = (char*)(BaseAddress + funcNameRva);
                                var name = Marshal.PtrToStringAnsi((IntPtr)funcName);
                                exportList.Add(new FunctionObject(name));
                            }
                        }
                    }
                }
            }
            return myObject;
        }

        public MyObject Load64Imports(MyObject myObject, string filePath, bool mappedAsImage)
        {
            var hLib = LoadLibrary(filePath);
            if (hLib == null)
            {
                var errorCode = GetLastError();
            }
            PeHeaderReader reader = new PeHeaderReader(filePath);

            List<ImportFunctionObject> objList = myObject.FunctionObjectList;
            
            Console.WriteLine("1st function list");
            unsafe
            {
                void* hMod = (void*)hLib;
              
                ulong size = 0;
                ulong BaseAddress = (ulong)hMod;

                if (hMod != null)
                {

                    IMAGE_IMPORT_DESCRIPTOR* pIID = (IMAGE_IMPORT_DESCRIPTOR*)Interop.ImageDirectoryEntryToData((void*)hMod, mappedAsImage, Interop.IMAGE_DIRECTORY_ENTRY_IMPORT, out size);

                    if (pIID != null)
                    {
                        // walk the array until find the end of the array
                        while (pIID->OriginalFirstThunk != 0)
                        {
                            try
                            {
                                //Name contains the RVA to the name of the dll. 
                                //Thus convert it to a virtual address first.
                                char* szName = (char*)(BaseAddress + pIID->Name);

                                IntPtr result = new IntPtr(szName);
                                string name = Marshal.PtrToStringAnsi(result);

                                if (!name.Contains("api-ms-win"))
                                {
                                    // value in OriginalFirstThunk is an RVA. 
                                    // convert it to virtual address.
                                    THUNK_DATA64* pThunkOrg = (THUNK_DATA64*)(BaseAddress + pIID->OriginalFirstThunk);
                                    while (pThunkOrg->AddressOfData != 0)
                                    {
                                        char* szImportName;
                                        ulong Ord;

                                        if ((pThunkOrg->Ordinal & 0x8000000000000000) > 0)
                                        {
                                            Ord = pThunkOrg->Ordinal & 0xffffffff;
                                        }
                                        else
                                        {
                                            IMAGE_IMPORT_BY_NAME64* pIBN = (IMAGE_IMPORT_BY_NAME64*)(BaseAddress + pThunkOrg->AddressOfData);

                                            if (!Interop.IsBadReadPtr((void*)pIBN, (ulong)sizeof(IMAGE_IMPORT_BY_NAME64)))
                                            {
                                                Ord = pIBN->Hint;
                                                szImportName = (char*)pIBN->Name;
                                                string sImportName = Marshal.PtrToStringAnsi((IntPtr)szImportName);

                                                UInt64 Address = pThunkOrg->Function;

                                                objList.Add(new ImportFunctionObject(sImportName, Address, name));
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }
                                        pThunkOrg++;
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                System.Diagnostics.Debug.WriteLine("An Access violation occured\n" +
                                                  "this seems to suggest the end of the imports section\n");
                                System.Diagnostics.Debug.WriteLine(e);
                            }
                            //pIID.size = 40;
                            pIID++;
                        }
                    }

                }


            }
            Console.WriteLine("2nd function list");

            return myObject;
        }

    }



    public class MyServer
    {
        public void Server()
        {

            Uri baseAddress = new Uri("net.pipe://localhost/Service64DLLLoader");

            // Create the ServiceHost.
            using (ServiceHost host = new ServiceHost(typeof(Service64), baseAddress))
            {
                //Add a service endpoint


                host.AddServiceEndpoint(typeof(ISimpleService64), new NetNamedPipeBinding(), "");

                // Enable metadata publishing.
                ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
                //   smb.HttpGetEnabled = true;
                smb.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
                host.Description.Behaviors.Add(smb);

                // Open the ServiceHost to start listening for messages. Since
                // no endpoints are explicitly configured, the runtime will create
                // one endpoint per base address for each service contract implemented
                // by the service.
                host.Open();
                Console.WriteLine("Service is host at Server 64 the needed server.............. New" + DateTime.Now.ToString());
                Console.WriteLine("The service 64 is ready at {0}", baseAddress);
                Console.WriteLine("Press <Enter> to stop the service.");
                Console.ReadLine();


                // Close the ServiceHost.
                host.Close();

            }
        }
    }
}
