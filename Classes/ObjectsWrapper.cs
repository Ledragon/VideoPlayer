using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Classes
{
    [XmlRoot("Library")]
    public class ObjectsWrapper
    {
        [XmlArray("Videos")]
        [XmlArrayItem("Video")]
        public List<Video> Videos { get; set; }
    }
}
