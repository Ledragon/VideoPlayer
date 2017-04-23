using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.PubSubEvents;
using VideoPlayer.Infrastructure;
using ViewModelBase = VideoPlayer.Infrastructure.ViewFirst.ViewModelBase;

namespace VideoPlayer.VideoListInteraction
{
    public class VideosListInteractionViewModel : ViewModelBase, IVideosListInteractionViewModel
    {
        private readonly IEventAggregator _eventAggregator;

        public VideosListInteractionViewModel(IEventAggregator eventAggregator)
        {
            this._eventAggregator = eventAggregator;
            this.PlayAllCommand = new DelegateCommand(this.PlayAll);
            this.AddAllCommand = new DelegateCommand(this.AddRange);
        }

        public ICommand AddAllCommand { get; }
        public ICommand PlayAllCommand { get; }

        private void PlayAll()
        {
            this._eventAggregator.GetEvent<PlayAllEvent>().Publish(null);
        }

        private void AddRange()
        {
            this._eventAggregator.GetEvent<OnAddVideoRangeRequest>().Publish(null);
        }
    }
}