using System;
using System.Xml.Serialization;

namespace VideoPlayer.Entities
{
    [Serializable]
    public class Tag
    {
        public Int32 Id { get; set; }
        [XmlAttribute]
        public String Value { get; set; }
    }
}
