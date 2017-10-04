//////////////////////////////////////////////////////////////////////////////////
// Repository.cs - Process that copies files from RepoStorage to BuilderStorage //
// and commands the builder to start.                                           //
// Ankur Kothari, CSE681 - Software Modeling and Analysis, Fall 2017            //
//////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.IO;
using SWTools;

namespace FederationServer
{
    public class Repository : CommunicatorBase
    {
        public Repository()
        {
            rcvQ = new BlockingQueue<Message>();
            start();
        }

        public string RepoStorage { get; set; } = "../../../Repository/RepoStorage";
        public string BuildStorage { get; set; } = "../../../Builder/BuilderStorage";
        public List<string> Files { get; set; } = new List<string>();

        public override void Execute()
        {
            CopyFilesFromRepoStorageToBuildStorage();
            environ.builder.Execute();
        }

        private void CopyFilesFromRepoStorageToBuildStorage()
        {
            if (!Directory.Exists(BuildStorage))
                Directory.CreateDirectory(BuildStorage);
            //read files from the repoStorage and move move to the buildStorage
            var tempFiles = Directory.GetFiles(RepoStorage, "*.*");
            for (var i = 0; i < tempFiles.Length; ++i)
                tempFiles[i] = Path.GetFullPath(tempFiles[i]);
            Files.AddRange(tempFiles);
            foreach (var file in Files)
            {
                Console.Write("\n  \"{0}\"", file);
                var fileName = Path.GetFileName(file);
                Console.Write("\n  sending \"{0}\" to \"{1}\"", fileName, BuildStorage);
                var result = SendFile(file);
                if (result)
                    Console.Write("\n  File sent");
                else
                    Console.Write("\n  File not sent");
            }
        }

        private bool SendFile(string fileSpec)
        {
            bool result;
            try
            {
                var fileName = Path.GetFileName(fileSpec);
                var destSpec = Path.Combine(BuildStorage, fileName ?? throw new InvalidOperationException());
                File.Copy(fileSpec, destSpec, true);
                result = true;
            }
            catch (Exception ex)
            {
                Console.Write("\n--{0}--", ex.Message);
                result = false;
            }
            return result;
        }
    }

    internal class TestRepository
    {
#if (TEST_Repository)
    static void Main(string[] args)
    {
      Console.Write("\n  Testing Repository Class");
      Console.Write("\n =======================\n");

      Client client = new Client();
      client.Execute();

      Repository repo = new Repository();
      repo.execute();
      Console.WriteLine();
    }
#endif
    }
}