using System.Collections.ObjectModel;
using System.Linq;
using Classes;
using Microsoft.Practices.Prism.PubSubEvents;
using VideoPlayer.Infrastructure;
using VideoPlayer.Services;

namespace VideoPlayer.ViewModels
{
    public class EditViewModel:ViewModelBase, IEditViewModel
    {
        private readonly IEventAggregator _eventAggregator;
        private Video _selectedVideo;

        public EditViewModel(ILibraryService libraryService, IEventAggregator eventAggregator)
        {
            this._eventAggregator = eventAggregator;
            this.Videos = new ObservableCollection<Video>(libraryService.GetObjectsFromFile().Videos.OrderBy(v=>v.Title));
        }

        public ObservableCollection<Video> Videos { get; private set; }

        public Video SelectedVideo
        {
            get { return this._selectedVideo; }
            set
            {
                if (Equals(value, this._selectedVideo)) return;
                this._selectedVideo = value;
                this._eventAggregator.GetEvent<VideoEditing>().Publish(this._selectedVideo);
                this.OnPropertyChanged();
            }
        }
    }

    public interface IEditViewModel
    {
        ObservableCollection<Video> Videos { get; }
        Video SelectedVideo { get; set; }
    }
}