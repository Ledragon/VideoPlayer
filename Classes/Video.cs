using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Windows.Media;
using System.IO;
using System.Threading;
using System.ComponentModel;
using System.Windows.Media.Imaging;
using System.Windows;

namespace Classes
{
    public class Video
    {
        public Video()
        {

        }

        public Video(String videoPath)
        {
            BackgroundWorker backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += backgroundWorker_DoWork;
            backgroundWorker.RunWorkerAsync(videoPath);

            this.Directory = System.IO.Path.GetDirectoryName(videoPath);
            this.FileName = videoPath;           
            this.Title = Path.GetFileNameWithoutExtension(videoPath);
            this.NumberOfViews = 0;
            this.Rating = 0;
        }

        void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            MediaPlayer mediaPlayer = new MediaPlayer();
            mediaPlayer.Open(new Uri((String)e.Argument));
            Int32 iterCount = 0;
            while (!mediaPlayer.NaturalDuration.HasTimeSpan && iterCount < 30)
            {
                Thread.Sleep(1000);
                iterCount++;
            }
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

        [XmlIgnore]
        public ImageSource Source { get; set; }
    }
}
