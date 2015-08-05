using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.PubSubEvents;
using VideoPlayer.Infrastructure;
using VideoPlayer.Services;

namespace HomeModule
{
    public class VideoPlayerViewModel : ViewModelBase, IVideoPlayerViewModel
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly ILibraryService _libraryService;
        private Visibility _isExitMenuVisible;
        private Visibility _isLoading;
        private String _loadingMessage;
        private Int32 _selectedTab;
        private WindowStyle _windowStyle;

        public VideoPlayerViewModel(IVideoPlayer videoPlayer, IEventAggregator eventAggregator,
            ILibraryService libraryService) : base(videoPlayer)
        {
            this.IsLoading = Visibility.Hidden;
            this.IsExitMenuVisible = Visibility.Hidden;

            this._eventAggregator = eventAggregator;
            this._libraryService = libraryService;
            this.GoToHomePageCommand = new DelegateCommand(this.SetHomePage);
            this.ToggleStyleCommand = new DelegateCommand(this.ToggleWindowStyle);
            this.CloseCommand = new DelegateCommand(this.CloseAsync);

            this._eventAggregator.GetEvent<GoToPage>().Subscribe(this.SetSelectedTab);
            this._eventAggregator.GetEvent<LibraryUpdating>()
                .Subscribe(message =>
                {
                    this.LoadingMessage = message;
                    this.IsLoading = Visibility.Visible;
                });
            this._eventAggregator.GetEvent<LibraryUpdated>()
                .Subscribe(payload => { this.IsLoading = Visibility.Hidden; });
        }

        public DelegateCommand GoToHomePageCommand { get; private set; }
        public DelegateCommand ToggleStyleCommand { get; private set; }
        public DelegateCommand WindowLoadedCommand { get; private set; }
        public DelegateCommand CloseCommand { get; private set; }

        public Int32 SelectedTab
        {
            get { return this._selectedTab; }
            set
            {
                if (value == this._selectedTab) return;
                this._selectedTab = value;
                this.OnPropertyChanged();
            }
        }

        public String LoadingMessage
        {
            get { return this._loadingMessage; }
            private set
            {
                if (value == this._loadingMessage) return;
                this._loadingMessage = value;
                this.OnPropertyChanged();
            }
        }

        public Visibility IsLoading
        {
            get { return this._isLoading; }
            set
            {
                if (value == this._isLoading) return;
                this._isLoading = value;
                this.OnPropertyChanged();
            }
        }

        public WindowStyle WindowStyle
        {
            get { return this._windowStyle; }
            set
            {
                if (value == this._windowStyle) return;
                this._windowStyle = value;
                this.OnPropertyChanged();
            }
        }

        public Visibility IsExitMenuVisible
        {
            get { return this._isExitMenuVisible; }
            set
            {
                if (value == this._isExitMenuVisible) return;
                this._isExitMenuVisible = value;
                this.OnPropertyChanged();
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
            if (this.SelectedTab != 0)
            {
                this.SetSelectedTab(0);
            }
        }

        private void SetSelectedTab(Int32 pageNumber)
        {
            this.SelectedTab = pageNumber;
        }

        private void ToggleWindowStyle()
        {
            this.WindowStyle = this.WindowStyle == WindowStyle.None ? WindowStyle.SingleBorderWindow : WindowStyle.None;
        }
    }
}