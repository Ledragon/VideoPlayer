using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Windows.Media;
using System.IO;
using System.Threading;

namespace Classes
{
    public class Video
    {
        public Video()
        {

        }

        public Video(String videoPath)
        {
            MediaPlayer mediaPlayer = new MediaPlayer();
            mediaPlayer.Open(new Uri(videoPath));
            this.Directory = System.IO.Path.GetDirectoryName(videoPath);
            this.FileName = videoPath;
            Int32 iterCount = 0;
            while (!mediaPlayer.NaturalDuration.HasTimeSpan && iterCount < 1)
            {
                //Thread.Sleep(100);
                iterCount++;
            }
            if (mediaPlayer.NaturalDuration.HasTimeSpan)
            {
                this.Length = mediaPlayer.NaturalDuration.TimeSpan;
            }
            this.Title = Path.GetFileNameWithoutExtension(videoPath);
            this.NumberOfViews = 0;
            this.Rating = 0;
            mediaPlayer.Close();
        }

        [XmlAttribute("FileName")]
        public String FileName { get; set; }

        [XmlAttribute("Title")]
        public String Title { get; set; }

        [XmlIgnore]
        public TimeSpan Length { get; set; }

        [XmlIgnore]
        private String _lengthString;
        
        [XmlAttribute("Length")]
        public String LengthString
        {
            get { return this.Length.ToString("hh\\:mm\\:ss"); }
            set { this._lengthString = this.Length.ToString(); }
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

        [XmlAttribute("DateAdded")]
        public DateTime DateAdded { get; set; }

        [XmlAttribute("LastPlayed")]
        public DateTime LastPlayed { get; set; }

        [XmlIgnore]
        private Uri _fileUri;

        [XmlIgnore]
        public Uri FileUri
        {
            get { return new Uri(this.FileName); }
            set { this._fileUri = new Uri(this.FileName); }
        }
    }
}
