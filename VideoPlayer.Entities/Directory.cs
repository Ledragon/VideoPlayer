using System;
using System.Xml.Serialization;

namespace VideoPlayer.Entities
{
    public class Directory
    {
        [XmlAttribute("DirectoryPath")]
        public String DirectoryPath { get; set; }

        [XmlAttribute("DirectoryName")]
        public String DirectoryName { get; set; }

        [XmlAttribute("IsIncludeSubdirectories")]
        public Boolean IsIncludeSubdirectories { get; set; }
    }
}
