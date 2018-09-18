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
        public byte[] bytes;
        public string name;
        public int bytesize;
        public bool check;
        public FileData(byte[] bytes, string name)
        {
            this.bytes = bytes;
            this.name = name;
            this.bytesize = bytes.Length;
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
                        FileData t = new FileData(tmp, files[i]);

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

                        if (i != j && !i.check && !j.check)
                        {
                            if (i.bytes.SequenceEqual(j.bytes))
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
            Console.WriteLine("Enter path to directory (use double slash):");
            string dir = Console.ReadLine();

            List<FileData> paths = getFileList(dir);

            Stopwatch sw = new Stopwatch();
            sw.Start();

            Console.WriteLine("================RESULT=============");

            int identical = printIdentical(paths);
            sw.Stop();

            Console.WriteLine("====================================");
            Console.WriteLine(paths.Count + " files checked");
            Console.WriteLine("====================================");
            Console.WriteLine(identical + " duplicates found");
            Console.WriteLine("====================================");
            Console.WriteLine(sw.Elapsed + " elapsed");

            Console.ReadKey();
        }
    }
}
