using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Classes
{
    [Serializable]
    public class Tag
    {
        [XmlAttribute]
        public String Value { get; set; }
    }
}
