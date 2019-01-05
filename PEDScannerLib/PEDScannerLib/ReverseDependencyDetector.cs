using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PEDScannerLib.Core
{
    class ReverseDependencyDetector
    {
        private HashSet<PortableExecutable> localPortableExecutables;

        private HashSet<PortableExecutable> globalPortableExecutables;

        private List<PortableExecutable> reverseDependecyList;

        private PortableExecutable target;

        //Method 1
        public bool IsEquals(object obj)
        {

            var pe_obj = obj as PortableExecutable;
            if (pe_obj == null)
            {
                return false;
            }

            try
            {
                string target_filePath = target.FilePath;
                string peobj_filePath = pe_obj.FilePath;

                byte[] array_1 = File.ReadAllBytes(target_filePath);
                byte[] array_2 = File.ReadAllBytes(peobj_filePath);

                int i;
                if (array_1.Length == array_2.Length)
                {
                    i = 0;
                    while (i < array_1.Length && (array_1[i] == array_2[i]))
                    {
                        i++;
                    }
                    if (i == array_1.Length)
                    {
                        return true;
                    }
                }
            }
            catch (IOException e)
            {
                Console.WriteLine(e);
            }
            return false;
        }



        private void SearchLocal(PortableExecutable target)
        {
            foreach (PortableExecutable portableExecutable in localPortableExecutables)
            {
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
                if (IsEquals(portableExecutable))
                {
                    containsTargetAsaDependecy = true;
                    break;
                }
            }

            return containsTargetAsaDependecy;
        }

        public List<PortableExecutable> getReverseDependecies()
        {
            return reverseDependecyList;
        }

        public void LoadLocal(String FilePath)
        {
            List<String> fileList = LoadDirectory(FilePath);
            foreach (String Path in fileList)
            {
                PortableExecutable portableExecutable = new PortableExecutable(Path);
                localPortableExecutables.Add(portableExecutable);
            }
            foreach (string d in Directory.GetDirectories(FilePath))
            {
                LoadLocal(d);
            }
        }

        public List<String> LoadDirectory(String FilePath)
        {
            List<String> fileList = new List<String>();
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

