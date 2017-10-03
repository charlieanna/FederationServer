using SWTools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public List<string> files { get; set; } = new List<string>();

        public override void processMessage(Message msg)
        {
            execute();
            msg.to = "repo";
            msg.from = "clnt";
            environ.builder.postMessage(msg);
        }

        private void execute()
        {
            CopyFilesFromRepoStorageToBuildStorage();
        }

        private void CopyFilesFromRepoStorageToBuildStorage()
        {
            if (!Directory.Exists(BuildStorage))
                Directory.CreateDirectory(BuildStorage);
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

        private bool sendFile(string fileSpec)
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
