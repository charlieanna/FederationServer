///////////////////////////////////////////////////////////////////////////
// DllLoader.cs - Demonstrate Robust loading and dynamic invocation of   //
//                Dynamic Link Libraries found in specified location     //
// ver 2 - tests now return bool for pass or fail                        //
//                                                                       //
// Jim Fawcett, CSE681 - Software Modeling and Analysis, Fall 2017       //
///////////////////////////////////////////////////////////////////////////


using System;
using System.IO;
using System.Reflection;

namespace FederationServer
{
    internal class DllLoaderExec
    {
        public DllLoaderExec(TestElement test)
        {
            Test = test;
        }

        public TestElement Test { get; set; }
        public static string testersLocation { get; set; } = ".";

        /*----< library binding error event handler >------------------*/
        /*
         *  This function is an event handler for binding errors when
         *  loading libraries.  These occur when a loaded library has
         *  dependent libraries that are not located in the directory
         *  where the Executable is running.
         */
        private static Assembly LoadFromComponentLibFolder(object sender, ResolveEventArgs args)
        {
            Console.Write("\n  called binding error event handler");
            var folderPath = testersLocation;
            var assemblyPath = Path.Combine(folderPath, new AssemblyName(args.Name).Name + ".dll");
            if (!File.Exists(assemblyPath)) return null;
            var assembly = Assembly.LoadFrom(assemblyPath);
            return assembly;
        }
        //----< load assemblies from testersLocation and run their tests >-----

        public string loadAndExerciseTesters()
        {
            var currentDomain = AppDomain.CurrentDomain;
            currentDomain.AssemblyResolve += LoadFromComponentLibFolder;

            try
            {
                // load each assembly found in testersLocation

                var files = Directory.GetFiles(testersLocation, Test.testDriver);
                foreach (var file in files)
                {
                    //Assembly asm = Assembly.LoadFrom(file);
                    var asm = Assembly.LoadFile(file);
                    var fileName = Path.GetFileName(file);
                    Console.Write("\n  loaded {0}", fileName);

                    // exercise each tester found in assembly

                    var types = asm.GetTypes();
                    foreach (var t in types)
                        // if type supports ITest interface then run test

                        if (t.GetInterface("CSTestDemo.ITest", true) != null)
                            if (!runSimulatedTest(t, asm))
                                Console.Write("\n  test {0} failed to run", t);
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return "Simulated Testing completed";
        }
        //
        //----< run tester t from assembly asm >-------------------------------

        private bool runSimulatedTest(Type t, Assembly asm)
        {
            var filestream = new FileStream("test.log",  FileMode.Append, FileAccess.Write);
            var streamwriter = new StreamWriter(filestream)
            {
                AutoFlush = true
            };
            var currentOut = Console.Out;
            try
            {
                Console.Write(
                    "\n  attempting to create instance of {0}", t
                );
                var obj = asm.CreateInstance(t.ToString());

                // announce test

                var method = t.GetMethod("say");
                if (method != null)
                    method.Invoke(obj, new object[0]);

                // run test

                var status = false;
                method = t.GetMethod("test");
                if (method != null)
                    status = (bool) method.Invoke(obj, new object[0]);
                else
                    Console.Write(
                        "\n\n  Could not find 'bool test() or say()' in the assembly.\n  Make sure it implements ITest\n  Test failed");

                Func<bool, string> act = pass =>
                {
                    if (pass)
                        return "passed";
                    return "failed";
                };
                
                Console.SetOut(streamwriter);

                Console.WriteLine("\n{0}  {1}\n", Test.testName, act(status));
                Console.WriteLine();
                streamwriter.Flush();
                Console.SetOut(currentOut);
                streamwriter.Close();
                filestream.Close();
                Console.WriteLine("Done");
            }
            catch (Exception ex)
            {
                Console.SetOut(currentOut);
                Console.Write("\n  test failed with message \"{0}\"", ex.Message);
                return false;
            }
            
            return true;
        }

        //
        //----< extract name of current directory without its parents ---------
    }
}