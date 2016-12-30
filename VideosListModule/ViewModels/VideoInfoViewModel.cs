﻿using System;
using System.IO;
using Classes;
using Microsoft.Practices.Prism.PubSubEvents;
using VideoPlayer.Infrastructure;
using VideosListModule.Views;

namespace VideosListModule
{
    public class VideoInfoViewModel : ViewModelBase, IVideoInfoViewModel
    {
        private readonly IEventAggregator _eventAggregator;
        private String _contactSheetPath;
        private Boolean _hasContactSheet;
        private Video _video;

        public VideoInfoViewModel(IEventAggregator eventAggregator, IVideoInfoView view) : base(view)
        {
            this._eventAggregator = eventAggregator;
            this._eventAggregator.GetEvent<VideoSelected>().Subscribe(video => this.Video = video);
        }

        public Video Video
        {
            get { return this._video; }
            set
            {
                if (Equals(value, this._video))
                {
                    return;
                }
                this._video = value;
                this.ContactSheetPath = this._video?.FileName + ".png";
                this.HasContactSheet = File.Exists(this.ContactSheetPath);
                this.OnPropertyChanged();
            }
        }

        public String ContactSheetPath
        {
            get { return this._contactSheetPath; }
            private set
            {
                if (value == this._contactSheetPath)
                {
                    return;
                }
                this._contactSheetPath = value;
                this.OnPropertyChanged();
            }
        }

        public Boolean HasContactSheet
        {
            get { return this._hasContactSheet; }
            private set
            {
                if (value == this._hasContactSheet)
                {
                    return;
                }
                this._hasContactSheet = value;
                this.OnPropertyChanged();
            }
        }
    }
}