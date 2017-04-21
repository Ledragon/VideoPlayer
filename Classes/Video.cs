using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using Classes.Annotations;
using Log;
using Microsoft.WindowsAPICodePack.Shell;
using ToolLib;

namespace Classes
{
    public class Video : INotifyPropertyChanged
    {
        private String _category;
        private DateTime _lastPlayed;
        private TimeSpan _length;
        private Int32 _numberOfViews;
        private UInt32 _rating;
        private Size _resolution;
        private ObservableCollection<Tag> _tags;
        private String _title;
        private Image _previewImage;

        public Video()
        {
        }

        public Video(String videoPath)
        {
            //this.GetVideoInfo(videoPath);
            this.Directory = Path.GetDirectoryName(videoPath);
            this.FileName = videoPath;
            try
            {
                if (File.Exists(videoPath))
                {
                    this.Title = Path.GetFileNameWithoutExtension(videoPath).Replace("%20", " ");
                    //this.NumberOfViews = 0;
                    this.Rating = 0;
                    this.Tags = new ObservableCollection<Tag>();
                    using (var shellFile = ShellFile.FromFilePath(videoPath))
                    {
                        var thumbnail = shellFile.Thumbnail.ExtraLargeBitmap;
                        this.PreviewImage = thumbnail;
                        var duration = shellFile.Properties.System.Media.Duration.Value;

                        Double nanoSeconds = 0;
                        if (Double.TryParse(duration.ToString(), out nanoSeconds))
                        {
                            var milliSeconds = nanoSeconds*0.0001;
                            this.Length = TimeSpan.FromMilliseconds(milliSeconds);
                        }
                        var rating = shellFile.Properties.System.Rating.Value;
                        UInt32 myRating = 0;
                        if (UInt32.TryParse(rating.ToString(), out myRating))
                        {
                            this.Rating = myRating;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                this.Logger().ErrorFormat(e.Message);
                this.Logger().ErrorFormat(e.StackTrace);
            }
        }

        [XmlAttribute("FileName")]
        public String FileName { get; set; }

        [XmlAttribute("Title")]
        public String Title
        {
            get { return this._title; }
            set
            {
                this._title = value;
                this.OnPropertyChanged();
            }
        }

        [XmlIgnore]
        public TimeSpan Length
        {
            get { return this._length; }
            set
            {
                this._length = value;
                this.OnPropertyChanged();
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
        public ObservableCollection<Tag> Tags
        {
            get { return this._tags; }
            set
            {
                if (Equals(value, this._tags)) return;
                this._tags = value;
                this.OnPropertyChanged();
            }
        }

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
                this.OnPropertyChanged();
            }
        }

        [XmlAttribute("Rating")]
        public UInt32 Rating
        {
            get { return this._rating; }
            set
            {
                this._rating = value;
                this.OnPropertyChanged();
            }
        }

        [XmlAttribute]
        public String Category
        {
            get { return this._category; }
            set
            {
                this._category = value;
                this.OnPropertyChanged();
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
                this.OnPropertyChanged();
            }
        }

        [XmlAttribute("SerializedImage")]
        public String SerializedImage { get; set; }

        [XmlIgnore]
        public Image PreviewImage
        {
            get
            {
                //if (this._previewImage == null)
                //{
                    var modifier = new ImageModifier();
                    var image = modifier.DeserializeFromBase64String(this.SerializedImage);
                    this._previewImage = image;
                //}
                return this._previewImage;
            }
            set
            {
                var modifier = new ImageModifier();
                var resized = value;
                if (resized.Width > 640)
                {
                    var ratio = (Double) value.Width/value.Height;
                    var newHeight = (640/ratio).ToString("0");
                    resized = modifier.ResizeImage(value, 640, Int32.Parse(newHeight));
                }
                this._previewImage = resized;
                this.SerializedImage = modifier.SerializeToBase64String(resized);
                this.OnPropertyChanged();
            }
        }

        public Size Resolution
        {
            get
            {
                if (this._resolution == Size.Empty && File.Exists(this.FileName))
                {
                    try
                    {
                        using (var shellFile = ShellFile.FromFilePath(this.FileName))
                        {
                            var video = shellFile.Properties.System.Video;
                            this._resolution = new Size(Convert.ToInt32(video.FrameWidth.Value),
                                Convert.ToInt32(video.FrameHeight.Value));
                        }
                    }
                    catch (Exception e)
                    {
                        this.Logger().Error(e.Message);
                    }
                }
                return this._resolution;
            }
            set
            {
                if (value.Equals(this._resolution)) return;
                this._resolution = value;
                this.OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] String propertyName = null)
        {
            var handler = this.PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}