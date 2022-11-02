using System;
using System.IO;
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
            {
                using (var xw = XmlWriter.Create(sw, new XmlWriterSettings { Indent = true }))
                {
                    xw.WriteStartDocument(true); // that bool parameter is called "standalone"

                    var namespaces = new XmlSerializerNamespaces();
                    namespaces.Add(String.Empty, String.Empty);

                    var xmlSerializer = new XmlSerializer(typeof(MovieNfo));
                    xmlSerializer.Serialize(xw, nfo);
                    File.WriteAllText(path, sw.ToString());
                }
            }
        }
    }
}
