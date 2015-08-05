using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public List<Video> Videos { get; set; }

        public ObjectsWrapper()
        {
            this.Directories = new ObservableCollection<Directory>();//<Directory>();
            this.Videos = new List<Video>();
        }
    }
}
