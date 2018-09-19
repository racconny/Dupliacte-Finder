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

    class Output
    {
        public int dups;
        public List<string> output;
        public Output(int dups, List<string> output)
        {
            this.dups = dups;
            this.output = output;
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

            Output printIdentical (List<FileData> files)
            {
                int duplicates = 0;
                List<string> res = new List<string>();
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
                        res.Add("A file " + i.name + " has " + copies.Count + " duplicates:");
                        duplicates++;
                        foreach (FileData item in copies)
                        {
                            duplicates++;
                            res.Add("------->" + item.name);
                        }
                    }
                }
                return new Output(duplicates, res);
            }

            //"C:\\Program Files\\nodejs"
            Console.WriteLine("Enter path to directory:");
            string dir = Console.ReadLine();

            Stopwatch f = new Stopwatch();
            f.Start();

            List<FileData> paths = getFileList(dir);

            f.Stop();
            Stopwatch c = new Stopwatch();

            Console.WriteLine("================RESULT=============");

            c.Start();

            Output identical = printIdentical(paths);
            c.Stop();

            foreach (string i in identical.output)
            {
                Console.WriteLine(i);
            }

            Console.WriteLine("====================================");
            Console.WriteLine(paths.Count + " files checked");
            Console.WriteLine("====================================");
            Console.WriteLine(identical.dups + " duplicates found");
            Console.WriteLine("====================================");
            Console.WriteLine(f.Elapsed + " took for getting file list");
            Console.WriteLine(c.Elapsed + " took for comparation");

            System.GC.Collect();
            Console.ReadKey();
        }
    }
}
