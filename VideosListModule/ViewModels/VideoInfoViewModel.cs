using Classes;
using Microsoft.Practices.Prism.PubSubEvents;
using VideoPlayer.Infrastructure;
using VideosListModule.Views;

namespace VideosListModule
{
    public class VideoInfoViewModel : ViewModelBase, IVideoInfoViewModel
    {
        private readonly IEventAggregator _eventAggregator;
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
                this.OnPropertyChanged();
            }
        }
    }

    public interface IVideoInfoViewModel : IViewModel
    {
        Video Video { get; set; }
    }
}