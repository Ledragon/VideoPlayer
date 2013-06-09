using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace VideoPlayer
{
    public class Video
    {
        [XmlAttribute("FileName")]
        public String  FileName { get; set; }

        [XmlAttribute("Title")]
        public String Title { get; set; }

        [XmlIgnore]
        public TimeSpan Length { get; set; }

        [XmlAttribute("Length")]
        public String LengthString
        {
            get { return this.Length.ToString(); }
            set { value = this.Length.ToString("HHmmss"); }
        }

        [XmlArray("Tags")]
        [XmlArrayItem("Tag")]
        public List<String> Tags { get; set; }

        public String Directory { get; set; }
        public String Preview { get; set; }
    }
}
