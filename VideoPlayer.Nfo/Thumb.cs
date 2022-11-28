using System;
using System.Xml.Serialization;

namespace VideoPlayer.Nfo
{
    public class Thumb
    {
        [XmlAttribute("aspect")]
        public String Aspect { get; set; }

        [XmlAttribute("preview")]
        public String Preview
        {
            get { return this.Path; }
            set { this.Path = value; }
        }

        [XmlText]
        public String Path { get; set; }
    }
}
