using System;
using System.Collections.ObjectModel;
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
        private ObservableCollection<Object> _exitMenu;
        private DelegateCommand _goToHomePageCommand;
        private Visibility _isExitMenuVisible;
        private Visibility _isLoading;
        private String _loadingMessage;
        private Int32 _selectedTab;
        private DelegateCommand _toggleExitMenuCommand;
        private DelegateCommand _toggleStyleCommand;
        private DelegateCommand _windowClosingCommand;
        private DelegateCommand _windowLoadedCommand;
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
            this.WindowClosingCommand = new DelegateCommand(this.WindowClosing);
            this.ToggleExitMenuCommand = new DelegateCommand(this.ToggleExitMenu);

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

        public DelegateCommand LoadCommand { get; set; }
        public DelegateCommand CleanCommand { get; set; }

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

        public DelegateCommand GoToHomePageCommand
        {
            get { return this._goToHomePageCommand; }
            set
            {
                if (Equals(value, this._goToHomePageCommand)) return;
                this._goToHomePageCommand = value;
                this.OnPropertyChanged();
            }
        }

        public DelegateCommand ToggleStyleCommand
        {
            get { return this._toggleStyleCommand; }
            set
            {
                if (Equals(value, this._toggleStyleCommand)) return;
                this._toggleStyleCommand = value;
                this.OnPropertyChanged();
            }
        }

        public DelegateCommand WindowClosingCommand
        {
            get { return this._windowClosingCommand; }
            set
            {
                if (Equals(value, this._windowClosingCommand)) return;
                this._windowClosingCommand = value;
                this.OnPropertyChanged();
            }
        }

        public DelegateCommand WindowLoadedCommand
        {
            get { return this._windowLoadedCommand; }
            set
            {
                if (Equals(value, this._windowLoadedCommand)) return;
                this._windowLoadedCommand = value;
                this.OnPropertyChanged();
            }
        }

        public DelegateCommand ToggleExitMenuCommand
        {
            get { return this._toggleExitMenuCommand; }
            set
            {
                if (Equals(value, this._toggleExitMenuCommand)) return;
                this._toggleExitMenuCommand = value;
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

        public ObservableCollection<Object> ExitMenu
        {
            get { return this._exitMenu; }
            set
            {
                if (Equals(value, this._exitMenu)) return;
                this._exitMenu = value;
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

        private void ToggleExitMenu()
        {
            if (this.IsExitMenuVisible == Visibility.Visible)
            {
                this.IsExitMenuVisible = Visibility.Hidden;
            }
            else
            {
                this.IsExitMenuVisible = Visibility.Visible;
                this._eventAggregator.GetEvent<CloseRequestedEvent>().Publish(null);
            }
        }

        private void WindowClosing()
        {
            this._libraryService.Save();
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