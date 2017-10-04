///////////////////////////////////////////////////////////////////////////
// Executive.cs - Process that starts all the other processes and commands //
// the client to start.                                                    //
// Ankur Kothari, CSE681 - Software Modeling and Analysis, Fall 2017       //
///////////////////////////////////////////////////////////////////////////

using FederationServer.Build;

namespace FederationServer
{
    internal class LibraryBuilder
    {
        public void build(string BuildStorage, TestElement testElement)
        {
            if (testElement.toolchain == "csharp")
            {
                var builder = new CSharpLibraryBuilder(BuildStorage, testElement);
                builder.Build();
            }
            else if (testElement.toolchain == "java")
            {
                var builder = new JavaLibraryBuilder(BuildStorage, testElement);
                builder.Build();
            }
        }
    }
}