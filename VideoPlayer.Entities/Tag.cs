using System;
using System.Xml.Serialization;

namespace VideoPlayer.Entities
{
    [Serializable]
    public class Tag
    {
        [XmlAttribute]
        public String Value { get; set; }
    }
}
