using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace VideoPlayer.Entities
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

        [XmlArray("Playlists")]
        public List<Playlist> PlayLists { get; set; }

        public ObjectsWrapper()
        {
            this.Directories = new ObservableCollection<Directory>();
            this.Videos = new List<Video>();
            if (this.PlayLists == null)
            {
                this.PlayLists = new List<Playlist>();
            }
        }
    }
}
