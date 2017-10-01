public class TestDriver
  {
    static boolean testTested1()
    {
      boolean result = true;
      Tested1 td1 = new Tested1();
      String value = td1.vowels();
      System.out.println("\n  td1.vowels() returned " + value);
      if (value != "aeiou")
      {
        result = false;
      }
      return result;
    }
    static boolean testTested2()
    {
      boolean result = true;
      Tested2 td2 = new Tested2();
      int value = td2.add(1, 2);
      System.out.println("\n  t2.add(1, 2) return " + 3);
      if (value != 3)
        result = false;
      return result;
    }
    
    public static void main(String args[]){
      boolean result1 = testTested1();
      boolean result2 = testTested2();
      System.out.println(result1 && result2);
    }
    public boolean test()
    {
      boolean result1 = testTested1();
      boolean result2 = testTested2();
      return result1 && result2;
    }
  }
