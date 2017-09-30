using FederationServer.Build;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FederationServer
{
    class LibraryBuilder
    {
        public void build(string BuildStorage, TestElement testElement)
        {
            if (testElement.toolchain == "csharp")
            {
                CSharpLibraryBuilder builder = new CSharpLibraryBuilder(BuildStorage, testElement);
                builder.build();
            }
            else if (testElement.toolchain == "java")
            {
                JavaLibraryBuilder builder = new JavaLibraryBuilder(BuildStorage, testElement);
                builder.build();
            }
        }
    }

        
}
