/////////////////////////////////////////////////////////////////////
// ChildProc - demonstrate creation of multiple .net processes     //
//                                                                 //
// Jim Fawcett, CSE681 - Software Modeling and Analysis, Fall 2017 //
/////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ChildBuilder
{
    class ChildBuilder
    {
        static void Main(string[] args)
        {
            Console.Title = nameof(ChildBuilder);
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.DarkBlue;

            Console.Write("\n  Demo Child Process");
            Console.Write("\n ====================");

            if (args.Count() == 0)
            {
                Console.Write("\n  please enter integer value on command line");
                return;
            }
            else
            {
                Console.Write("\n  Hello from child #{0}\n\n", args[0]);
            }
            Console.Write("\n  Press key to exit");
            Console.ReadKey();
            Console.Write("\n  ");
        }
    }
}
