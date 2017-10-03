/////////////////////////////////////////////////////////////////////
// TestDriver.cs - demonstration test driver                       //
//                                                                 //
// Jim Fawcett, CSE681 - Software Modeling and Analysis, Fall 2017 //
/////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSTestDemo
{
  interface ITest
  {
    bool test();
  }
  public class TestDriver1 : ITest
  {
    bool testTested1()
    {
      bool result = true;
      Tested3 td1 = new Tested3();
      string value = td1.vowels();
      Console.Write("\n  td1.vowels() returned {0}", value);
      if (value != "aeiou")
      {
        result = false;
      }
      return result;
    }
    
    public bool test()
    {
      bool result1 = testTested1();
      return result1;
    }
  }
}
