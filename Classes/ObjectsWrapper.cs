using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Classes
{
    [XmlRoot("Library")]
    public class ObjectsWrapper
    {
        [XmlArray("Directories")]
        [XmlArrayItem("Directory")]
        public ObservableCollection<Directory> Directories { get; set; }

        [XmlArray("Videos")]
        [XmlArrayItem("Video")]
        public ObservableCollection<Video> Videos { get; set; }
    }
}
