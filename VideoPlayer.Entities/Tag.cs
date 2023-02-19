using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace VideoPlayer.Entities
{
    [Serializable]
    public class Tag
    {
        public Int32 Id { get; set; }

        [XmlAttribute]
        public String Value { get; set; }

        [XmlIgnore]
        public ICollection<Video> Videos { get; set; }

        [XmlIgnore]
        public List<TagVideo> TagVideos { get; set; }
    }
}
