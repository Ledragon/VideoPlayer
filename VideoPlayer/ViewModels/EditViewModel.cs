using System.ComponentModel;
using System.Windows.Data;
using Classes;
using Microsoft.Practices.Prism.PubSubEvents;
using VideoPlayer.Infrastructure;
using VideoPlayer.Services;
using VideosListModule.ViewModels;

namespace VideoPlayer.ViewModels
{
    public class EditViewModel : ViewModelBase, IEditViewModel
    {
        private readonly IEventAggregator _eventAggregator;
        private Video _selectedVideo;

        public EditViewModel(ILibraryService libraryService, IEventAggregator eventAggregator)
        {
            this._eventAggregator = eventAggregator;
            this.Videos = new VideosCollectionView(libraryService.GetObjectsFromFile().Videos, 0);
            this.Videos.Sort(new SortDescription("Category", ListSortDirection.Ascending));
            this.Videos.GroupDescriptions.Add(new PropertyGroupDescription("Category"));
            //this.SelectedVideo = this.Videos.FirstOrDefault();
        }

        public VideosCollectionView Videos { get; }

        public Video SelectedVideo
        {
            get { return this._selectedVideo; }
            set
            {
                if (Equals(value, this._selectedVideo))
                {
                    return;
                }
                this._selectedVideo = value;
                this._eventAggregator.GetEvent<VideoEditing>().Publish(this._selectedVideo);
                this.OnPropertyChanged();
            }
        }
    }
}