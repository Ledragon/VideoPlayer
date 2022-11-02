using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace VideoPlayer.Nfo
{
    [XmlRoot(ElementName = "movie")]
    public class MovieNfo
    {
        [XmlElement(ElementName = "title")]
        public String Title { get; set; }
        [XmlElement(ElementName = "originaltitle")]
        public String OriginalTitle { get; set; }
        public Int32 UserRating { get; set; }
        [XmlElement(ElementName ="runtime")]
        public Int32 Runtime { get; set; }
        [XmlArray("fanart")]
        [XmlArrayItem("thumb")]
        public List<Thumb> FanArt { get; set; }
        public Int32 PlayCount { get; set; }
        public DateTime LastPlayed { get; set; }
        public Set Set { get; set; }
        public DateTime DateAdded { get; set; }
        [XmlElement(ElementName = "thumb")]
        public Thumb Thumb { get; set; }
        [XmlElement(ElementName = "genre")]
        public List<String> Genres { get; set; }
        [XmlElement(ElementName = "tag")]
        public List<String> Tags { get; set; }
        public MovieNfo()
        {
            this.Genres = new List<String>();
            this.Tags = new List<String>();
        }
    }
}
