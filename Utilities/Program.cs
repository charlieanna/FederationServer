using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FederationServer
{
    public static class ToAndFromXml
    {
        //----< serialize object to XML >--------------------------------

        static public string ToXml(this object obj)
        {
            // suppress namespace attribute in opening tag

            XmlSerializerNamespaces nmsp = new XmlSerializerNamespaces();
            nmsp.Add("", "");

            var sb = new StringBuilder();
            try
            {
                var serializer = new XmlSerializer(obj.GetType());
                using (StringWriter writer = new StringWriter(sb))
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

        static public T FromXml<T>(this string xml)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(new StringReader(xml));
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
        public static void title(this string aString, char underline = '-')
        {
            Console.Write("\n  {0}", aString);
            Console.Write("\n {0}", new string(underline, aString.Length + 2));
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
        }
    }
}
