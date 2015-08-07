using System;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.PubSubEvents;
using VideoPlayer.Infrastructure;

namespace VideoPlayer.ViewModels
{
    public class VideosTabControlViewModel : ViewModelBase
    {
        private Cursor _cursor;
        private Boolean _filterGridVisibility;
        private Boolean _isCategoryGridVisible;
        private Boolean _isPlayListVisible;
        private Int32 _selectedIndex;

        public VideosTabControlViewModel(IEventAggregator eventAggregator)
        {
            this.FilterGridVisibility = false;
            this.IsPlayListVisible = false;
            this.IsCategoryGridVisible = true;

            this.InitCommands();

            eventAggregator.GetEvent<PlayedEvent>().Subscribe(this.SwitchToFullScreen);
            eventAggregator.GetEvent<OnStop>().Subscribe(this.SwitchToWindowMode);
        }

        public ICommand SwitchCategoryGridVisibilityCommand { get; private set; }
        public ICommand SwitchPlaylistVisibilityCommand { get; private set; }
        public ICommand SwitchToWindowCommand { get; private set; }
        public ICommand SwitchToFullScreenCommand { get; private set; }
        public ICommand SwitchFilterGridVisibilityCommand { get; private set; }

        public Boolean IsCategoryGridVisible
        {
            get { return this._isCategoryGridVisible; }
            set
            {
                if (value == this._isCategoryGridVisible) return;
                this._isCategoryGridVisible = value;
                this.OnPropertyChanged();
            }
        }

        public Boolean IsPlayListVisible
        {
            get { return this._isPlayListVisible; }
            set
            {
                if (value == this._isPlayListVisible) return;
                this._isPlayListVisible = value;
                this.OnPropertyChanged();
            }
        }

        public Cursor Cursor
        {
            get { return this._cursor; }
            set
            {
                if (Equals(value, this._cursor)) return;
                this._cursor = value;
                this.OnPropertyChanged();
            }
        }

        public Boolean FilterGridVisibility
        {
            get { return this._filterGridVisibility; }
            set
            {
                if (!value.Equals(this._filterGridVisibility))
                {
                    this._filterGridVisibility = value;
                    this.OnPropertyChanged();
                }
            }
        }

        public Int32 SelectedIndex
        {
            get { return this._selectedIndex; }
            set
            {
                if (value == this._selectedIndex) return;
                this._selectedIndex = value;
                this.OnPropertyChanged();
            }
        }

        private void InitCommands()
        {
            this.SwitchFilterGridVisibilityCommand = new DelegateCommand(this.SwitchFilterGridVisibility);
            this.SwitchToFullScreenCommand = new DelegateCommand(this.SwitchToFullScreen);
            this.SwitchToWindowCommand = new DelegateCommand(this.SwitchToWindowMode);
            this.SwitchPlaylistVisibilityCommand = new DelegateCommand(this.SwitchPlayListVisibility);
            this.SwitchCategoryGridVisibilityCommand =
                new DelegateCommand(() => this.IsCategoryGridVisible = !this.IsCategoryGridVisible);
        }

        private void SwitchPlayListVisibility()
        {
            this.IsPlayListVisible = !this.IsPlayListVisible;
        }

        private void SwitchFilterGridVisibility()
        {
            this.FilterGridVisibility = !this.FilterGridVisibility;
        }

        private void SwitchToWindowMode()
        {
            this.SwitchToWindowMode(null);
        }

        private void SwitchToWindowMode(Object dummy)
        {
            if (this.SelectedIndex != 0)
            {
                this.SelectedIndex = 0;
                this.Cursor = Cursors.Arrow;
            }
        }

        private void SwitchToFullScreen()
        {
            this.SwitchToFullScreen(null);
        }

        private void SwitchToFullScreen(Object dummy)
        {
            if (this.SelectedIndex != 1)
            {
                this.SelectedIndex = 1;
            }
        }
    }
}