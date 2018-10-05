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
        public String Title { get; set; }
        public String  OriginalTitle { get; set; }
        public Int32 UserRating { get; set; }
        public Int32 Runtime { get; set; }
        public List<Thumb> FanArt { get; set; }
        public Int32 PlayCount { get; set; }
        public DateTime LastPlayed { get; set; }
        public Set Set { get; set; }
        public DateTime DateAdded { get; set; }
        public Thumb Thumb { get; set; }

    }

    public class Thumb
    {
        [XmlAttribute]
        public String Aspect { get; set; }
        [XmlAttribute]
        public String Preview { get; set; }
    }

    public class Set
    {
        public String Name { get; set; }
        public String Overview { get; set; }
    }
}
