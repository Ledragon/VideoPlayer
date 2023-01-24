using LeDragon.Log.Standard;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Xml.Serialization;

namespace VideoPlayer.Entities
{
    public class Video
    {
        private DateTime _lastPlayed;
        private TimeSpan _length;
        private List<Tag> _tags;

        public Video()
        {
        }

        public Video(String videoPath)
        {
            this.Directory = Path.GetDirectoryName(videoPath);
            this.FileName = videoPath;
            try
            {
                if (File.Exists(videoPath))
                {
                    this.Title = Path.GetFileNameWithoutExtension(videoPath).Replace("%20", " ");
                    this.NumberOfViews = 0;
                    this.Rating = 0;
                    this.Tags = new List<Tag>();
                }
            }
            catch (Exception e)
            {
                this.Logger().ErrorFormat(e.Message);
                this.Logger().ErrorFormat(e.StackTrace);
            }
        }

        public Int32 Id { get; set; }

        [XmlAttribute("FileName")]
        public String FileName { get; set; }

        [XmlAttribute("Title")]
        public String Title { get; set; }

        [NotMapped]
        [XmlIgnore]
        public TimeSpan Length
        {
            get { return this._length; }
            set{this._length = value;}
        }

        [XmlAttribute("Length")]
        public String LengthString
        {
            get { return this.Length.ToString("hh\\:mm\\:ss"); }
            set { this.Length = TimeSpan.Parse(value); }
        }

        [XmlArray("Tags")]
        [XmlArrayItem("Tag")]
        public List<Tag> Tags
        {
            get { return this._tags; }
            set
            {
                if (Equals(value, this._tags))
                {
                    return;
                }

                this._tags = value;
            }
        }

        [XmlAttribute("Directory")]
        public String Directory { get; set; }

        [NotMapped]
        [XmlAttribute("Preview")]
        public String Preview { get; set; }

        [XmlAttribute("NumberOfViews")]
        public Int32 NumberOfViews { get; set; }

        [XmlAttribute]
        public UInt32 Rating { get; set; }

        [NotMapped]
        [XmlAttribute]
        public String Category { get; set; }

        [XmlIgnore]
        public DateTime DateAdded { get; set; }

        [NotMapped]
        [XmlAttribute("DateAdded")]
        public String DateAddedString
        {
            get { return this.DateAdded.ToString("yyyyMMdd_HHmmss"); }
            set { this.DateAdded = DateTime.ParseExact(value, "yyyyMMdd_HHmmss", null); }
        }

        [XmlAttribute("LastPlayed")]
        public DateTime LastPlayed
        {
            get { return this._lastPlayed; }
            set
            {
                if (this._lastPlayed != value)
                {
                    this._lastPlayed = value;
                }
            }
        }

        [NotMapped]
        [XmlAttribute("SerializedImage")]
        public String SerializedImage { get; set; }

        public List<Thumbnail> Thumbnails { get; set; }
    }
}