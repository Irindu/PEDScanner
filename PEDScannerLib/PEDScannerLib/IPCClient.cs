using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using Objects;
using PEDScannerLib.Objects;

namespace Service64Proxy
{
    [ServiceContract()]
    public interface ISimpleService64
    {


        [OperationContract]
        MyObject Load64Imports(MyObject myObject, string filePath, bool mappedAsImage);

        [OperationContract]
        ExportObject Load64Exports(ExportObject myObject, string filePath, bool mappedAsImage);

    }

    // WCF create proxy for ISimpleCalculator using ClientBase
    public class Service64 :
    ClientBase<ISimpleService64>,

    ISimpleService64
    {

        public MyObject Load64Imports(MyObject myObject, string filePath, bool mappedAsImage)
        {
            return base.Channel.Load64Imports(myObject, filePath, mappedAsImage);
        }

        public ExportObject Load64Exports(ExportObject myObject, string filePath, bool mappedAsImage)
        {
            return base.Channel.Load64Exports(myObject, filePath, mappedAsImage);
        }

    }
}

namespace Objects
{


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
    //public class FunctionObject
    //{
    //    public string function;
    //    public FunctionObject()
    //    {
            
    //    }
    //    public FunctionObject(string functionName)
    //    {
    //        this.function = functionName;
    //    }
        
    //}

    //public class ImportFunctionObject : FunctionObject
    //{
        
    //    public UInt64 baseAddress;
    //    public string dependency;
    //    public ImportFunctionObject()
    //    {
            
    //    }
       
    //    public ImportFunctionObject(string functionName, UInt64 baseAddress, string dependency) : base(functionName)
    //    {
    //        this.baseAddress = baseAddress;
    //        this.dependency = dependency;
    //    }
 
    //}
   
}






