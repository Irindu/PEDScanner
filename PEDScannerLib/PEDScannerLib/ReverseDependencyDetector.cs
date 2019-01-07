using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PEDScannerLib.Core
{
public class ReverseDependencyDetector
    {
        private HashSet<PortableExecutable> localPortableExecutables;

        private List<PortableExecutable> reverseDependecyList;

        public ReverseDependencyDetector() {
            reverseDependecyList = new List<PortableExecutable>();
            localPortableExecutables = new HashSet<PortableExecutable>();
        }


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
                string target_filePath = pe_obj.FilePath;
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

        public List<PortableExecutable> Process(String FilePath, PortableExecutable target) {
            this.LoadLocal(FilePath);
            this.SearchLocal(target);

            return this.reverseDependecyList;
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

        private bool SearchIndividualPE(PortableExecutable candidate, PortableExecutable target)
        {
            bool containsTargetAsaDependecy = false;
            if (candidate!= null && target != null && candidate.Dependencies.Count > 0) 
            foreach (PortableExecutable dependency in candidate.Dependencies)
            {
                if (target.Equals(dependency))
                {
                    containsTargetAsaDependecy = true;
                    break;
                }
            }

            return containsTargetAsaDependecy;
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

        private List<String> LoadDirectory(String FilePath)
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

