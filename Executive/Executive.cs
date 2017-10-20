///////////////////////////////////////////////////////////////////////////
// Executive.cs - Process that starts all the other processes and commands //
// the client to start.                                                    //
// Ankur Kothari, CSE681 - Software Modeling and Analysis, Fall 2017       //
///////////////////////////////////////////////////////////////////////////

using FederationServer;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

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
        private static void Main1()
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

        static bool createProcess(int i)
        {
            using (var proc = new Process())
            {
                var fileName = "..\\..\\..\\ChildBuilder\\bin\\debug\\ChildBuilder.exe";
                string absFileSpec = Path.GetFullPath(fileName);

                Console.Write("\n  attempting to start {0}", absFileSpec);
                string commandline = i.ToString();
                try
                {
                    Process.Start(fileName, commandline);
                }
                catch (Exception ex)
                {
                    Console.Write("\n  {0}", ex.Message);
                    return false;
                }
                return true;
            }
        }
        static void Main(string[] args)
        {
            Console.Title = "SpawnProc";
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.DarkBlue;

            Console.Write("\n  Demo Parent Process");
            Console.Write("\n =====================");

            if (args.Count() == 0)
            {
                Console.Write("\n  please enter number of processes to create on command line");
                return;
            }
            else
            {
                int count = Int32.Parse(args[0]);
                for (int i = 1; i <= count; ++i)
                {
                    if (createProcess(i))
                    {
                        Console.Write(" - succeeded");
                    }
                    else
                    {
                        Console.Write(" - failed");
                    }
                }
            } 
            Console.Write("\n  Press key to exit");
            Console.ReadKey();
            Console.Write("\n  ");
        }
    }
}