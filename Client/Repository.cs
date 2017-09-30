﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Repository
    {
        public Repository()
        {
            CopyFilesFromRepoStorageToBuildStorage();
        }
        public static string RepoStorage { get; set; } = "../../RepoStorage";
        public static string BuildStorage { get; set; } = "../../BuilderStorage";
        public static string TestStorage { get; set; } = "../../TestStorage";
        public static List<string> files { get; set; } = new List<string>();
        public void CopyFilesFromRepoStorageToBuildStorage()
        {
            //read files from the repoStorage and move move to the buildStorage
            string[] tempFiles = Directory.GetFiles(RepoStorage, "*.*");
            for (int i = 0; i < tempFiles.Length; ++i)
            {
                tempFiles[i] = Path.GetFullPath(tempFiles[i]);
            }
            files.AddRange(tempFiles);
            foreach (string file in files)
            {
                Console.Write("\n  \"{0}\"", file);
                string fileName = Path.GetFileName(file);
                Console.Write("\n  sending \"{0}\" to \"{1}\"", fileName, BuildStorage);
                sendFile(file);
            }

        }

        public static bool sendFile(string fileSpec)
        {
            try
            {
                string fileName = Path.GetFileName(fileSpec);
                string destSpec = Path.Combine(BuildStorage, fileName);
                File.Copy(fileSpec, destSpec, true);
                return true;
            }
            catch (Exception ex)
            {
                Console.Write("\n--{0}--", ex.Message);
                return false;
            }
        }


    }
}
