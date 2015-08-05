using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.PubSubEvents;
using VideoPlayer.Infrastructure;
using VideoPlayer.Services;

namespace HomeModule
{
    public class HomePageViewModel : ViewModelBase, IHomePageViewModel
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly ILibraryService _libraryService;
        private ICommand _cleanCommand;
        private ICommand _goToSettingsCommand;
        private ICommand _goToVideosCommand;
        private ICommand _loadCommand;

        public HomePageViewModel(IHomePage homePage, IEventAggregator eventAggregator, ILibraryService libraryService):base(homePage)
        {
            this._eventAggregator = eventAggregator;
            this._libraryService = libraryService;
            this.GoToSettingsCommand = new DelegateCommand(this.GoToSettings);
            this.GoToVideosCommand = new DelegateCommand(this.GoToVideos);
            this.CleanCommand=new DelegateCommand(this.Clean);
            this.LoadCommand=new DelegateCommand(this.Load);
        }

        private void Load()
        {
            this._eventAggregator.GetEvent<LibraryUpdating>().Publish(null);
            this._libraryService.Update();
            this._eventAggregator.GetEvent<LibraryUpdated>().Publish(null);
        }

        private void Clean()
        {
            this._eventAggregator.GetEvent<LibraryUpdating>().Publish(null);
            this._libraryService.Clean();
            this._eventAggregator.GetEvent<LibraryUpdated>().Publish(null);
        }

        public ICommand GoToSettingsCommand
        {
            get { return this._goToSettingsCommand; }
            set
            {
                if (Equals(value, this._goToSettingsCommand)) return;
                this._goToSettingsCommand = value;
                this.OnPropertyChanged();
            }
        }

        public ICommand GoToVideosCommand
        {
            get { return this._goToVideosCommand; }
            set
            {
                if (Equals(value, this._goToVideosCommand)) return;
                this._goToVideosCommand = value;
                this.OnPropertyChanged();
            }
        }

        public ICommand LoadCommand
        {
            get { return this._loadCommand; }
            set
            {
                if (Equals(value, this._loadCommand)) return;
                this._loadCommand = value;
                this.OnPropertyChanged();
            }
        }

        public ICommand CleanCommand
        {
            get { return this._cleanCommand; }
            set
            {
                if (Equals(value, this._cleanCommand)) return;
                this._cleanCommand = value;
                this.OnPropertyChanged();
            }
        }

        private void GoToSettings()
        {
            this._eventAggregator.GetEvent<GoToPage>().Publish(2);
        }

        private void GoToVideos()
        {
            this._eventAggregator.GetEvent<GoToPage>().Publish(1);
        }
    }
}