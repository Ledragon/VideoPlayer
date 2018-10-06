using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace VideoPlayer.Nfo
{
    public class NfoSerializer : INfoSerializer
    {
        public void Serialize(MovieNfo nfo, String path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            using (var sw = new Utf8StringWriter())
            using (var xw = XmlWriter.Create(sw, new XmlWriterSettings { Indent = true }))
            {
                xw.WriteStartDocument(true); // that bool parameter is called "standalone"

                var namespaces = new XmlSerializerNamespaces();
                namespaces.Add(String.Empty, String.Empty);

                var xmlSerializer = new XmlSerializer(typeof(MovieNfo));
                xmlSerializer.Serialize(xw, nfo);
                File.WriteAllText(path, sw.ToString());
                //Console.WriteLine(sw.ToString());
            }
            //var serializer = new XmlSerializer(typeof(MovieNfo));
            //var stream = File.Create(path);
            //serializer.Serialize(stream, nfo);
            //stream.Close();
        }
    }
}
