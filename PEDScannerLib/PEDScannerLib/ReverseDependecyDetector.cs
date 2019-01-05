using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PEDScannerLib.Core
{
    class ReverseDependecyDetector
    {
        private HashSet<PortableExecutable> localPortableExecutables;

        private HashSet<PortableExecutable> globalPortableExecutables;

        private List<PortableExecutable> reverseDependecyList;

        private PortableExecutable target;

        private void SearchLocal(PortableExecutable target) {
            foreach (PortableExecutable portableExecutable in localPortableExecutables){
                if (SearchIndividualPE(portableExecutable, target))
                {
                    reverseDependecyList.Add(portableExecutable);
                }
            }
        }
        //Define EQ of PE on baseClass
        private bool SearchIndividualPE(PortableExecutable portableExecutable, PortableExecutable target)
        {
            bool containsTargetAsaDependecy = false;
            foreach (PortableExecutable dependency in portableExecutable.Dependencies)
            {
                if (dependency.Name == target.Name)
                {
                    containsTargetAsaDependecy = true;
                    break;
                }
            }

            return containsTargetAsaDependecy;
        } 

        public List<PortableExecutable> getReverseDependecies() {
            return reverseDependecyList;
        }

        public void LoadLocal(String FilePath) {
            LoadDirectory(FilePath);
            foreach (string d in Directory.GetDirectories(FilePath))
            {
                LoadDirectory(d);
            }
           }

        public List<String> LoadDirectory(String FilePath)
        {
            List <String> fileList = new List<String>();
            try
            {
                foreach (string file in Directory.GetFiles(FilePath, "*.dll"))
                {
                    string extension = Path.GetExtension(file);
                    if (extension != null && (extension.Equals(".dll")))
                    {
                        fileList.Add(file);
                    }
                }

                foreach (string file in Directory.GetFiles(FilePath, "*.exe"))
                {
                    string extension = Path.GetExtension(file);
                    if (extension != null && (extension.Equals(".exe")))
                    {
                        fileList.Add(file);
                    }
                }
            }
            catch (Exception e)
            {

            }

            return fileList;
        }
    }
}

