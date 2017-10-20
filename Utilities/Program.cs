///////////////////////////////////////////////////////////////////////////
// Executive.cs - Process that starts all the other processes and commands //
// the client to start.                                                    //
// Jim Fawcett, CSE681 - Software Modeling and Analysis, Fall 2017       //
///////////////////////////////////////////////////////////////////////////




using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace FederationServer
{
    public static class ToAndFromXml
    {
        //----< serialize object to XML >--------------------------------

        public static string ToXml(this object obj)
        {
            // suppress namespace attribute in opening tag

            var nmsp = new XmlSerializerNamespaces();
            nmsp.Add("", "");

            var sb = new StringBuilder();
            try
            {
                var serializer = new XmlSerializer(obj.GetType());
                using (var writer = new StringWriter(sb))
                {
                    serializer.Serialize(writer, obj, nmsp);
                }
            }
            catch (Exception ex)
            {
                Console.Write("\n  exception thrown:");
                Console.Write("\n  {0}", ex.Message);
            }
            return sb.ToString();
        }
        //----< deserialize XML to object >------------------------------

        public static T FromXml<T>(this string xml)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                using (var stringReader = new StringReader(xml))
                {
                    return (T) serializer.Deserialize(stringReader);
                }
            }
            catch (Exception ex)
            {
                Console.Write("\n  deserialization failed\n  {0}", ex.Message);
                return default(T);
            }
        }
    }

    public static class Utilities
    {
        public static void Title(this string aString, char underline = '-')
        {
            Console.Write("\n  {0}", aString);
            Console.Write("\n {0}", new string(underline, aString.Length + 2));
        }
    }
}