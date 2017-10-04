///////////////////////////////////////////////////////////////////////////
// Executive.cs - Process that starts all the other processes and commands //
// the client to start.                                                    //
// Ankur Kothari, CSE681 - Software Modeling and Analysis, Fall 2017       //
///////////////////////////////////////////////////////////////////////////

using FederationServer;
using System;
using System.IO;
namespace Executive
{
    public class Executive : CommunicatorBase
    {
        public static void Load()
        {
            environ.client = new Client();
            environ.repo = new Repository();
            environ.builder = new Builder();
            environ.testHarness = new TestHarness();
        }

        public static void Start()
        {
            environ.client.Execute();
        }

        public static void CleanDirectories()
        {
            var testStorage = "../../../TestHarness/TestStorage";
            var buildStorage = "../../../Builder/BuilderStorage";
            CleanDirectory(buildStorage);
            CleanDirectory(testStorage);
        }

        private static void CleanDirectory(string directory)
        {
            if (Directory.Exists(directory))
            {
                var di = new DirectoryInfo(directory);

                foreach (var file in di.GetFiles())
                    file.Delete();
            }
        }
    }

    public class TestMsgPass
    {
        private static void Main()
        {
            try
            {
                Console.Write("\n  Starting Federation Server");
                Console.Write("\n ================================================");

                Executive.Load(); // builds federation components
                Executive.CleanDirectories(); // Empties the directories for each new execution
                Executive.Start(); // starts federation processing
                Console.Write("\n\n");
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: '{0}'", e.Message);
            }
        }
    }
}