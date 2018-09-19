using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace Duplicate_finder
{
    class FileData
    {
        public string name;
        public long size;
        public bool check;
        public FileData(string name)
        {
            this.name = name;
            this.size = new FileInfo(name).Length;
            this.check = false;
        }
    }

    class Program
    {

        static void Main(string[] args)
        {
            List<FileData> getFileList(string path)
            {
                string[] files = Directory.GetFiles(path);
                string[] dirs = Directory.GetDirectories(path);
                List<FileData> filedata = new List<FileData>();

                if (files.Length != 0)
                {
                    for (int i = 0; i < files.Length; i++)
                    {
                        byte[] tmp = File.ReadAllBytes(files[i]);
                        FileData t = new FileData(files[i]);

                        filedata.Add(t);
                    }
                }

                if (dirs.Length != 0)
                {
                    for (int i = 0; i < dirs.Length; i++)
                    {
                        string[] tmp = dirs[i].Split('\\');
                        filedata.AddRange(getFileList(dirs[i]));
                    }
                }

                return filedata;

            }

            int printIdentical (List<FileData> files)
            {
                int duplicates = 0;
                foreach(FileData i in files)
                {

                  List<FileData> copies = new List<FileData>();

                    foreach (FileData j in files)
                    {

                        if (i != j && !i.check && !j.check && i.size == j.size)
                        {
                            if (File.ReadAllBytes(i.name).SequenceEqual(File.ReadAllBytes(j.name)))
                            {
                                j.check = true;
                                copies.Add(j);
                            }
                        }

                    }
                    if (copies.Count != 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("A file {0} has {1} duplicate(s):", i.name, copies.Count);
                        Console.ForegroundColor = ConsoleColor.White;
                        duplicates++;
                        foreach (FileData item in copies)
                        {
                            duplicates++;
                            Console.WriteLine("-------" + item.name);
                        }
                    }
                }
                return duplicates;
            }

            //"C:\\Program Files\\nodejs"
            Console.WriteLine("Enter path to directory:");
            string dir = Console.ReadLine();

            Stopwatch sw = new Stopwatch();
            sw.Start();

            List<FileData> paths = getFileList(dir);

            Console.WriteLine("================RESULT=============");

            int identical = printIdentical(paths);
            sw.Stop();

            Console.WriteLine("====================================");
            Console.WriteLine(paths.Count + " files checked");
            Console.WriteLine("====================================");
            Console.WriteLine(identical + " duplicates found");
            Console.WriteLine("====================================");
            Console.WriteLine(sw.Elapsed + " elapsed");
            System.GC.Collect();
            Console.ReadKey();
        }
    }
}
