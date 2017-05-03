using System;
using System.Windows;
using HomeModule;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.Regions;
using VideoPlayer.Infrastructure;
using VideoPlayer.PlaylistManagement;
using VideoPlayer.Services;
using VlcPlayer;
using ViewModelBase = VideoPlayer.Infrastructure.ViewFirst.ViewModelBase;

namespace VideoPlayer.ViewModels
{
    public class VideoPlayerViewModel : ViewModelBase, IVideoPlayerViewModel
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly ILibraryService _libraryService;
        private readonly IRegionManager _regionManager;
        private Visibility _isExitMenuVisible;
        private Visibility _isLoading;
        private String _loadingMessage;
        private Int32 _selectedTab;

        public VideoPlayerViewModel(IEventAggregator eventAggregator,
            ILibraryService libraryService, IRegionManager regionManager)
        {
            this.IsLoading = Visibility.Hidden;
            this.IsExitMenuVisible = Visibility.Hidden;

            this._eventAggregator = eventAggregator;
            this._libraryService = libraryService;
            this._regionManager = regionManager;
            this.GoToHomePageCommand = new DelegateCommand(this.SetHomePage);
            this.ToggleStyleCommand = new DelegateCommand(this.ToggleWindowStyle);
            this.CloseCommand = new DelegateCommand(this.CloseAsync);

            this.NavigateCommand = new DelegateCommand<Object>(this.Navigate);
            ApplicationCommands.NavigateCommand.RegisterCommand(this.NavigateCommand);

            eventAggregator.GetEvent<PlayCompleted>()
                .Subscribe(dummy =>
                    ApplicationCommands.NavigateCommand.Execute(typeof(PlayListManagementView))
                );

            eventAggregator.GetEvent<OnStop>()
                .Subscribe(dummy =>
                    ApplicationCommands.NavigateCommand.Execute(typeof(PlayListManagementView))
                );
            eventAggregator.GetEvent<OnPlayPlaylistRequest>()
                .Subscribe(video =>
                {
                    ApplicationCommands.NavigateCommand.Execute(typeof(Player));
                });
        }

        public DelegateCommand<Object> NavigateCommand { get; }
        public DelegateCommand GoToHomePageCommand { get; }
        public DelegateCommand ToggleStyleCommand { get; }
        public DelegateCommand CloseCommand { get; }

        public String LoadingMessage
        {
            get { return this._loadingMessage; }
            private set
            {
                if (value == this._loadingMessage)
                {
                    return;
                }
                this._loadingMessage = value;
                this.OnPropertyChanged();
            }
        }

        public Visibility IsLoading
        {
            get { return this._isLoading; }
            set
            {
                if (value == this._isLoading)
                {
                    return;
                }
                this._isLoading = value;
                this.OnPropertyChanged();
            }
        }

        public Visibility IsExitMenuVisible
        {
            get { return this._isExitMenuVisible; }
            set
            {
                if (value == this._isExitMenuVisible)
                {
                    return;
                }
                this._isExitMenuVisible = value;
                this.OnPropertyChanged();
            }
        }

        private void Navigate(Object navigatePath)
        {
            if (navigatePath != null)
            {
                this._regionManager.RequestNavigate(RegionNames.ContentRegion, navigatePath.ToString());
            }
        }

        private async void CloseAsync()
        {
            this.IsExitMenuVisible = Visibility.Visible;
            await this._libraryService.SaveAsync();
            this._eventAggregator.GetEvent<CloseRequestedEvent>().Publish(null);
        }

        private void SetHomePage()
        {
            this.Navigate(typeof (HomePage));
        }

        private void ToggleWindowStyle()
        {
            //this.WindowStyle = this.WindowStyle == WindowStyle.None ? WindowStyle.SingleBorderWindow : WindowStyle.None;
        }
    }
}