using System;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using System.Windows.Media;
using System.IO;
using System.ComponentModel;
using ToolLib;

namespace Classes
{
    public class Video:INotifyPropertyChanged
    {
        public Video()
        {

        }

        public Video(String videoPath)
        {
            this.GetVideoInfo(videoPath);
            this.Directory = Path.GetDirectoryName(videoPath);
            this.FileName = videoPath;
            this.Title = Path.GetFileNameWithoutExtension(videoPath);
            this.NumberOfViews = 0;
            this.Rating = 0;
        }

        private void GetVideoInfo(String videoPath)
        {
            MediaPlayer mediaPlayer = new MediaPlayer();
            mediaPlayer.Open(new Uri(videoPath));
            //Int32 iterCount = 0;
            //while (!mediaPlayer.NaturalDuration.HasTimeSpan && iterCount < 10)
            //{
            //Thread.Sleep(1000);
            //    iterCount++;
            //}
            if (mediaPlayer.NaturalDuration.HasTimeSpan)
            {
                this.Length = mediaPlayer.NaturalDuration.TimeSpan;
            }

            //mediaPlayer.Pause();
            //mediaPlayer.Position = TimeSpan.FromSeconds(10);
            //RenderTargetBitmap rtb = new RenderTargetBitmap(60, 60, 72, 72, PixelFormats.Prgba64);
            //DrawingVisual dv = new DrawingVisual();
            //using (DrawingContext dc = dv.RenderOpen())
            //{
            //    dc.DrawVideo(mediaPlayer,new Rect(0,0,60,60));
            //}
            //rtb.Render(dv);

            //BitmapFrame frame = BitmapFrame.Create(rtb).GetCurrentValueAsFrozen() as BitmapFrame;
            //BitmapEncoder encoder = new JpegBitmapEncoder();
            //encoder.Frames.Add(frame as BitmapFrame);
            //MemoryStream memoryStream = new MemoryStream();
            //encoder.Save(memoryStream);

            mediaPlayer.Close();
        }

        [XmlAttribute("FileName")]
        public String FileName { get; set; }
        
        private String _title;
        [XmlAttribute("Title")]
        public String Title 
        {
            get 
            { 
                return this._title; 
            }
            set
            {
                this._title = value;
                this.NotifyPropertyChanged("Title");
            }
        }

        private TimeSpan _length;
        [XmlIgnore]
        public TimeSpan Length
        {
            get
            {
                return this._length;
            }
            set
            {
                this._length = value;
                this.NotifyPropertyChanged("Length");
            }
        }

        [XmlAttribute("Length")]
        public String LengthString
        {
            get
            {
                return this.Length.ToString("hh\\:mm\\:ss");
            }
            set
            {
                this.Length = TimeSpan.Parse(value);
            }
        }

        [XmlArray("Tags")]
        [XmlArrayItem("Tag")]
        public ObservableCollection<String> Tags { get; set; }

        [XmlAttribute("Directory")]
        public String Directory { get; set; }

        [XmlAttribute("Preview")]
        public String Preview { get; set; }

        private Int32 _numberOfViews;
        [XmlAttribute("NumberOfViews")]
        public Int32 NumberOfViews 
        {
            get
            {
                return this._numberOfViews;
            }
            set
            {
                this._numberOfViews = value;
                this.NotifyPropertyChanged("NumberOfViews");
            }
        }

        private Int32 _rating;
        [XmlAttribute("Rating")]
        public Int32 Rating
        {
            get
            {
                return this._rating;
            }
            set
            {
                this._rating = value;
                this.NotifyPropertyChanged("Rating");
            }
        }

        [XmlIgnore]
        public DateTime DateAdded { get; set; }

        [XmlAttribute("DateAdded")]
        public String DateAddedString
        {
            get { return this.DateAdded.ToString("yyyyMMdd_HHmmss"); }
            set { this.DateAdded = DateTime.ParseExact(value, "yyyyMMdd_HHmmss", null); }
        }

        private DateTime _lastPlayed;
        [XmlAttribute("LastPlayed")]
        public DateTime LastPlayed
        {
            get
            {
                return this._lastPlayed;
            }
            set
            {
                this._lastPlayed = value;
                this.NotifyPropertyChanged("LastPlayed");
            }
        }
        
        [XmlAttribute("SerializedImage")]
        public String SerializedImage { get; set; }

        [XmlIgnore]
        public System.Drawing.Image PreviewImage
        {
            get
            {
                ImageModifier modifier = new ImageModifier();
                return modifier.DeserializeFromBase64String(this.SerializedImage);
                //return GetValue(PreviewImageProperty) as System.Drawing.Image;
            }
            set
            {
                ImageModifier modifier = new ImageModifier();
                this.SerializedImage = modifier.SerializeToBase64String(value);
                this.NotifyPropertyChanged("PreviewImage");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
