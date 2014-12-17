using Microsoft.Practices.Prism.PubSubEvents;
using VideoPlayer.Infrastructure;

namespace Module
{
    public class VideoFilterGridViewModel : ViewModelBase, IVideoFilterGridViewModel
    {
        private readonly IEventAggregator _eventAggregator;

        public VideoFilterGridViewModel(IVideoFilterGrid videoFilterGrid, IEventAggregator eventAggregator) : base(videoFilterGrid)
        {
            this._eventAggregator = eventAggregator;
        }
    }
}