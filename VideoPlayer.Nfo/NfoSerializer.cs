using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace VideoPlayer.Nfo
{
    public class NfoSerializer
    {
        public void Serialize(MovieNfo nfo, String path)
        {
            var serializer = new XmlSerializer(typeof(MovieNfo));
            var stream = File.Create(path);
            serializer.Serialize(stream, nfo);
            stream.Close();
        }
    }
}
