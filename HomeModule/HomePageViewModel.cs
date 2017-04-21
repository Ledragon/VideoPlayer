using System.Runtime.CompilerServices;
using System.Windows.Input;
using ManageLibraryModule;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.PubSubEvents;
using PlaylistModule;
using VideoPlayer.Infrastructure;
using VideoPlayer.Services;
using VideosListModule;
using VideosPageModule;
using ApplicationCommands = VideoPlayer.Infrastructure.ApplicationCommands;

namespace HomeModule
{
    public class HomePageViewModel : ViewModelBase, IHomePageViewModel
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly ILibraryService _libraryService;

        public HomePageViewModel(IHomePage homePage, IEventAggregator eventAggregator, ILibraryService libraryService)
            : base(homePage)
        {
            this._eventAggregator = eventAggregator;
            this._libraryService = libraryService;
            this.GoToSettingsCommand = new DelegateCommand(this.GoToSettings);
            this.GoToVideosCommand = new DelegateCommand(this.GoToVideos);
            //this.GoToVideosCommand = (dummy)=>ApplicationCommands.NavigateCommand.Execute(this.View.GetType()); //new DelegateCommand(this.GoToVideos);
            this.CleanCommand = new DelegateCommand(this.Clean);
            this.LoadCommand = new DelegateCommand(this.LoadAsync);
            this.ManageCommand = new DelegateCommand(this.GoToManage);
        }

        public ICommand GoToSettingsCommand { get; }
        public ICommand GoToVideosCommand { get; }
        public ICommand LoadCommand { get; }
        public ICommand CleanCommand { get; }
        public ICommand ManageCommand { get; }

        private async void LoadAsync()
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
            var videos =  this._libraryService.Update();
            this._eventAggregator.GetEvent<LibraryUpdated>().Publish(videos);
        }

        private async void CleanAsync()
        {
            this._eventAggregator.GetEvent<LibraryUpdating>().Publish("Cleaning");
            await this._libraryService.CleanAsync();
            this._eventAggregator.GetEvent<LibraryUpdated>().Publish(null);
        }

        private void GoToSettings()
        {
            this._eventAggregator.GetEvent<GoToPage>().Publish(2);
        }

        private void GoToManage()
        {
            //this._eventAggregator.GetEvent<GoToPage>().Publish(3);
            ApplicationCommands.NavigateCommand.Execute(typeof(EditView));

        }

        private void GoToVideos()
        {
            //this._eventAggregator.GetEvent<GoToPage>().Publish(1);
            ApplicationCommands.NavigateCommand.Execute(typeof(VideosPage));
        }
    }
}