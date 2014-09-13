﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Xml.Serialization;
using Log;
using Microsoft.WindowsAPICodePack.Shell;
using ToolLib;

namespace Classes
{
    public class Video : INotifyPropertyChanged
    {
        private string _category;
        private DateTime _lastPlayed;
        private TimeSpan _length;
        private Int32 _numberOfViews;
        private UInt32 _rating;
        private String _title;

        public Video()
        {
        }

        public Video(String videoPath)
        {
            //this.GetVideoInfo(videoPath);
            this.Directory = Path.GetDirectoryName(videoPath);
            this.FileName = videoPath;
            this.Title = Path.GetFileNameWithoutExtension(videoPath).Replace("%20", " ");
            //this.NumberOfViews = 0;
            this.Rating = 0;
            this.Tags = new ObservableCollection<Tag>();
            using (ShellFile shellFile = ShellFile.FromFilePath(videoPath))
            {
                Bitmap thumbnail = shellFile.Thumbnail.ExtraLargeBitmap;
                this.PreviewImage = thumbnail;
                ulong? duration = shellFile.Properties.System.Media.Duration.Value;

                Double nanoSeconds = 0;
                if (Double.TryParse(duration.ToString(), out nanoSeconds))
                {
                    double milliSeconds = nanoSeconds*0.0001;
                    this.Length = TimeSpan.FromMilliseconds(milliSeconds);
                }
                uint? rating = shellFile.Properties.System.Rating.Value;
                UInt32 myRating = 0;
                if (UInt32.TryParse(rating.ToString(), out myRating))
                {
                    this.Rating = myRating;
                }
            }
        }

        /*
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
        */

        [XmlAttribute("FileName")]
        public String FileName { get; set; }

        [XmlAttribute("Title")]
        public String Title
        {
            get { return this._title; }
            set
            {
                this._title = value;
                this.NotifyPropertyChanged("Title");
            }
        }

        [XmlIgnore]
        public TimeSpan Length
        {
            get { return this._length; }
            set
            {
                this._length = value;
                this.NotifyPropertyChanged("Length");
            }
        }

        [XmlAttribute("Length")]
        public String LengthString
        {
            get { return this.Length.ToString("hh\\:mm\\:ss"); }
            set { this.Length = TimeSpan.Parse(value); }
        }

        [XmlArray("Tags")]
        [XmlArrayItem("Tag")]
        public ObservableCollection<Tag> Tags { get; set; }

        [XmlAttribute("Directory")]
        public String Directory { get; set; }

        [XmlAttribute("Preview")]
        public String Preview { get; set; }

        [XmlAttribute("NumberOfViews")]
        public Int32 NumberOfViews
        {
            get { return this._numberOfViews; }
            set
            {
                this._numberOfViews = value;
                this.NotifyPropertyChanged("NumberOfViews");
            }
        }

        [XmlAttribute("Rating")]
        public UInt32 Rating
        {
            get { return this._rating; }
            set
            {
                this._rating = value;
                this.NotifyPropertyChanged("Rating");
            }
        }

        [XmlAttribute]
        public String Category
        {
            get { return this._category; }
            set
            {
                this._category = value;

                this.NotifyPropertyChanged("Category");
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

        [XmlAttribute("LastPlayed")]
        public DateTime LastPlayed
        {
            get { return this._lastPlayed; }
            set
            {
                this._lastPlayed = value;
                this.NotifyPropertyChanged("LastPlayed");
            }
        }

        [XmlAttribute("SerializedImage")]
        public String SerializedImage { get; set; }

        [XmlIgnore]
        public Image PreviewImage
        {
            get
            {
                var modifier = new ImageModifier();
                var image = modifier.DeserializeFromBase64String(this.SerializedImage);
                return image;

                //return GetValue(PreviewImageProperty) as System.Drawing.Image;
            }
            set
            {
                var modifier = new ImageModifier();
                var resized = value;
                if (resized.Width > 640)
                {
                    Double ratio = (Double)value.Width/value.Height;
                    var newHeight = (640/ratio).ToString("0");
                    resized = modifier.ResizeImage(value, 640, Int32.Parse(newHeight));
                }
                this.SerializedImage = modifier.SerializeToBase64String(resized);
                this.NotifyPropertyChanged("PreviewImage");
            }
        }

        //[XmlIgnore]
        //public String Resolution
        //{
        //    get
        //    {
        //        String resolution = String.Empty;
        //        try
        //        {
        //            using (ShellFile shellFile = ShellFile.FromFilePath(this.FileName))
        //            {
        //                resolution = (shellFile.Properties.System.Video.FrameWidth.Value ?? 0) + "*" +
        //                             (shellFile.Properties.System.Video.FrameHeight.Value ?? 0);
        //            }

        //        }
        //        catch (Exception e)
        //        {

        //        }
        //        return resolution;
        //    }
        //}

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}