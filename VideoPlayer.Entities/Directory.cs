using System;
using System.Collections.Generic;
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

        public List<Video> Videos { get; set; }
    }
}
