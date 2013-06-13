using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Classes
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
