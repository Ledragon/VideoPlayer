using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Windows.Media;
using System.IO;

namespace VideoPlayer
{
    public class Video
    {
        public Video()
        {

        }

        public Video(String videoPath)
        {
            //MediaPlayer mediaPlayer = new MediaPlayer();
            //mediaPlayer.Open(new Uri(videoPath));
            this.Directory = System.IO.Path.GetDirectoryName(videoPath);
            this.FileName = Path.GetFileNameWithoutExtension(videoPath);
            //this.Length = mediaPlayer.NaturalDuration.TimeSpan;
            this.Title = this.FileName;
            this.NumberOfViews = 0;
            this.Rating = 0;
            //mediaPlayer.Close();
        }

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
            set { value = this.Length.ToString(); }
        }

        [XmlArray("Tags")]
        [XmlArrayItem("Tag")]
        public List<String> Tags { get; set; }

        [XmlAttribute("Directory")]        
        public String Directory { get; set; }

        [XmlAttribute("Preview")]
        public String Preview { get; set; }

        [XmlAttribute("NumberOfViews")]
        public Int32 NumberOfViews { get; set; }

        [XmlAttribute("Rating")]
        public Int32 Rating { get; set; }
    }
}
