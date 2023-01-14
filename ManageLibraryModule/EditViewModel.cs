using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Classes;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.PubSubEvents;
using VideoPlayer.Infrastructure;
using VideoPlayer.Nfo;
using VideoPlayer.Services;
using VideosListModule.ViewModels;
using ViewModelBase = VideoPlayer.Infrastructure.ViewFirst.ViewModelBase;

namespace ManageLibraryModule
{
    public class EditViewModel : ViewModelBase, IEditViewModel
    {
        private readonly ILibraryService _libraryService;
        private readonly IEventAggregator _eventAggregator;
        private VideoViewModel _selectedVideo;
        private VideosCollectionView _videos;

        public EditViewModel(ILibraryService libraryService, IEventAggregator eventAggregator, INfoService nfoService)
        {
            this._libraryService = libraryService;
            this._eventAggregator = eventAggregator;
            var collection = this._libraryService.GetObjectsFromFile().Videos;
            this.Refresh(collection.Select(v => new VideoViewModel(v)).ToList());
            this.UpdateCommand = new DelegateCommand(this.UpdateAsync);
            this.CleanCommand = new DelegateCommand(this.Clean);

            this.ToJsonCommand = new DelegateCommand(() => { libraryService.ToJson((this.Videos.SourceCollection as IEnumerable<VideoViewModel>).Select(v => v.Video)); });

            this.CreateNfoCommand = new DelegateCommand(() =>
            {
                nfoService.CreateNfo(collection);
            });

            this._eventAggregator.GetEvent<LibraryUpdated>()
                .Subscribe(newList =>
                {
                    this.Refresh(newList.Select(v=>new VideoViewModel(v)).ToList());
                });
        }

        private void Refresh(List<VideoViewModel> collection)
        {
            this.Videos = new VideosCollectionView(collection, 0);
            //this.Videos.Sort(new SortDescription("Category", ListSortDirection.Ascending));
            this.Videos.Sort(new SortDescription("DateAdded", ListSortDirection.Descending));
        }

        public ICommand UpdateCommand { get; }
        public ICommand CleanCommand { get; }
        public ICommand ToJsonCommand { get; }
        public ICommand CreateNfoCommand { get; }

        public VideosCollectionView Videos
        {
            get { return this._videos; }
            private set
            {
                if (Equals(value, this._videos))
                {
                    return;
                }
                this._videos = value;
                this.OnPropertyChanged();
            }
        }

        public VideoViewModel SelectedVideo
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
                this._eventAggregator.GetEvent<SetVideo>().Publish(this._selectedVideo);
                this.OnPropertyChanged();
            }
        }


        private async void UpdateAsync()
        {
            this._eventAggregator.GetEvent<LibraryUpdating>().Publish("Loading");
            var videos = await this._libraryService.UpdateAsync();
            this._eventAggregator
                .GetEvent<LibraryUpdated>()
                .Publish(videos);
        }

        private void Clean()
        {
            this._eventAggregator.GetEvent<LibraryUpdating>().Publish("Cleaning");
            this._libraryService.Clean();
            var videos = this._libraryService.Update();
            this._eventAggregator.GetEvent<LibraryUpdated>().Publish(videos);
        }

        //private async void CleanAsync()
        //{
        //    this._eventAggregator.GetEvent<LibraryUpdating>().Publish("Cleaning");
        //    await this._libraryService.CleanAsync();
        //    this._eventAggregator.GetEvent<LibraryUpdated>().Publish(null);
        //}

    }
}