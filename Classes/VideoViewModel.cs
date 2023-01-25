using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using Classes.Annotations;
using LeDragon.Log.Standard;
using Microsoft.WindowsAPICodePack.Shell;
using ToolLib;
using VideoPlayer.Entities;

namespace Classes
{
    public class VideoViewModel : INotifyPropertyChanged
    {
        private readonly Video _video;
        private Image _previewImage;
        private Size _resolution;

        public Video Video => this._video;

        public VideoViewModel(Video video)
        {
            this._video = video;
            var videoPath = video.FileName;
            this.Directory = Path.GetDirectoryName(videoPath);
            try
            {
                var wvie = new WindowsVideoInfoExtractor();
                wvie.SetShellInfo(this);
            }
            catch (Exception e)
            {
                this.Logger().ErrorFormat(e.Message);
                this.Logger().ErrorFormat(e.StackTrace);
            }
        }

        public String FileName { get => this._video.FileName; set => this._video.FileName = value; }

        public String Title
        {
            get { return this._video.Title; }
            set
            {
                this._video.Title = value;
                this.OnPropertyChanged();
            }
        }

        public TimeSpan Length
        {
            get { return TimeSpan.Parse(this._video.LengthString); }
            set
            {
                this._video.LengthString = value.ToString("hh\\:mm\\:ss");
                this.OnPropertyChanged();
            }
        }

        public ObservableCollection<Tag> Tags
        {
            get { return new ObservableCollection<Tag>(this._video.Tags); }
        }

        public String Directory
        {
            get => this._video.DirectoryPath;
            set
            {
                if (this._video.DirectoryPath != value)
                {
                    this._video.DirectoryPath = value;
                    this.OnPropertyChanged();
                }
            }
        }

        public String Preview { get; set; }

        public Int32 NumberOfViews
        {
            get { return this._video.NumberOfViews; }
            set
            {
                this._video.NumberOfViews = value;
                this.OnPropertyChanged();
            }
        }

        public UInt32 Rating
        {
            get { return this._video.Rating; }
            set
            {
                this._video.Rating = value;
                this.OnPropertyChanged();
            }
        }

        public String Category
        {
            get { return this._video.Category; }
            set
            {
                this._video.Category = value;
                this.OnPropertyChanged();
            }
        }

        public DateTime DateAdded { get => this._video.DateAdded; set => this._video.DateAdded = value; }

        public DateTime LastPlayed
        {
            get { return this._video.LastPlayed; }
            set
            {
                this._video.LastPlayed = value;
                this.OnPropertyChanged();
            }
        }

        public String SerializedImage { get => this._video.SerializedImage; set => this._video.SerializedImage = value; }

        public Image PreviewImage
        {
            get
            {
                if (this._previewImage == null)
                {
                    var modifier = new ImageModifier();
                    var image = modifier.DeserializeFromBase64String(this.SerializedImage);
                    this._previewImage = image;
                }
                return this._previewImage;
            }
            set
            {
                var modifier = new ImageModifier();
                var resized = value;
                if (resized.Width > 640)
                {
                    var ratio = (Double)value.Width / value.Height;
                    var newHeight = (640 / ratio).ToString("0");
                    resized = modifier.ResizeImage(value, 640, Int32.Parse(newHeight));
                }
                this._previewImage = resized;
                this.SerializedImage = modifier.SerializeToBase64String(resized);
                this.OnPropertyChanged();
            }
        }

        public Boolean HasContactSheet
        {
            get { return File.Exists(this.FileName + ".png"); }
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