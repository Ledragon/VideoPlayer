using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.PubSubEvents;
using VideoPlayer.Infrastructure;
using VideoPlayer.Views;

namespace VideoPlayer.ViewModels
{
    public class VideosListInteractionViewModel : Infrastructure.ViewModelBase, IVideosListInteractionViewModel
    {
        private readonly IEventAggregator _eventAggregator;

        public VideosListInteractionViewModel(IVideosListInteractionView view, IEventAggregator eventAggregator)
            : base(view)
        {
            this._eventAggregator = eventAggregator;
            this.PlayAllCommand = new DelegateCommand(this.PlayAll);
            this.AddAllCommand = new DelegateCommand(this.AddRange);
        }

        public ICommand AddAllCommand { get; private set; }
        public ICommand PlayAllCommand { get; private set; }

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