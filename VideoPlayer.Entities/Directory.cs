using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace VideoPlayer.Entities
{
    public class Directory
    {
        public Int32 Id { get; set; }

        [XmlAttribute("DirectoryPath")]
        public String DirectoryPath { get; set; }

        [XmlAttribute("DirectoryName")]
        public String DirectoryName { get; set; }

        [XmlAttribute("IsIncludeSubdirectories")]
        public Boolean IsIncludeSubdirectories { get; set; }

        public List<Video> Videos { get; set; }

        public MediaType MediaType { get; set; }
    }
}
