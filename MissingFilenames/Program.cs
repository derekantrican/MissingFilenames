using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Drawing;

namespace MissingFilenames
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"C:\Users\derek.antrican\Downloads\Photobucket\";

            DirectoryInfo dir = new DirectoryInfo(path);

            List<FileInfo> files = dir.GetFiles().ToList();
            files = files.OrderByDescending(s => int.Parse(Regex.Match(s.Name, @"\d+").Value)).Reverse().ToList();
            CheckForCorruptFiles(files);
            CheckForMissingFilenames(files);

            Console.WriteLine("=================================");
            Console.WriteLine(" Finished. Press any key to exit");
            Console.WriteLine("=================================");
            Console.ReadLine(); //Pause
        }

        private static void CheckForCorruptFiles(List<FileInfo> files)
        {
            foreach (FileInfo file in files)
            {
                try
                {
                    Bitmap temp = new Bitmap(file.FullName);
                    temp.Dispose();
                }
                catch
                {
                    Console.WriteLine("There is a problem with " + file.Name);
                }
            }
        }

        private static void CheckForMissingFilenames(List<FileInfo> files)
        {
            int index = Convert.ToInt32(Regex.Match(files[0].Name, @"\d+").Value);

            foreach (FileInfo file in files)
            {
                if (file.Name.Contains("-1"))
                    continue;

                int curIndex = Convert.ToInt32(Regex.Match(file.Name, @"\d+").Value);

                while (curIndex > index + 1)
                {
                    string missingFileName = Regex.Replace(file.Name, @"\d+", (index + 1).ToString());
                    Console.WriteLine("missing file: " + missingFileName);

                    index++;
                }

                index = curIndex;
            }
        }
    }
}
