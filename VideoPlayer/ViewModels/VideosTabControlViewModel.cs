using System;
using System.Windows;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.PubSubEvents;
using VideoPlayer.Infrastructure;

namespace VideoPlayer.ViewModels
{
    public class VideosTabControlViewModel : ViewModelBase
    {
        private readonly IEventAggregator _eventAggregator;
        private ICommand _clearFilterCommand;
        private Cursor _cursor;
        private Visibility _filterGridVisibility;
        private Int32 _numberOfVideos;
        private ICommand _playAllCommand;
        private Int32 _selectedIndex;
        private ICommand _showFilterGridCommand;
        private ICommand _switchToFullScreenCommand;
        private ICommand _switchToWindowCommand;

        public VideosTabControlViewModel(IEventAggregator eventAggregator)
        {
            this._eventAggregator = eventAggregator;
            this.FilterGridVisibility = Visibility.Collapsed;

            this.ShowFilterGridCommand = new DelegateCommand(this.ShowFilterGrid);
            this.SwitchToFullScreenCommand = new DelegateCommand(this.SwitchToFullScreen);
            this.SwitchToWindowCommand = new DelegateCommand(this.SwitchToWindowMode);
            this.PlayAllCommand = new DelegateCommand(this.PlayAll);
            this.AddAllCommand = new DelegateCommand(this.AddRange);

            this._eventAggregator.GetEvent<PlayedEvent>().Subscribe(this.SwitchToFullScreen);
            this._eventAggregator.GetEvent<OnStop>().Subscribe(this.SwitchToWindowMode);
            this._eventAggregator.GetEvent<FilterChangedEvent>().Subscribe(i =>
            {
                this.NumberOfVideos = i;
            });
        }

        public ICommand PlayAllCommand
        {
            get { return this._playAllCommand; }
            set
            {
                if (Equals(value, this._playAllCommand)) return;
                this._playAllCommand = value;
                this.OnPropertyChanged();
            }
        }

        public Int32 NumberOfVideos
        {
            get
            {
                return this._numberOfVideos;
            }
            set
            {
                if (value == this._numberOfVideos) return;
                this._numberOfVideos = value;
                this.OnPropertyChanged();
            }
        }

        public ICommand ClearFilterCommand
        {
            get { return this._clearFilterCommand; }
            set
            {
                if (Equals(value, this._clearFilterCommand)) return;
                this._clearFilterCommand = value;
                this.OnPropertyChanged();
            }
        }

        public ICommand SwitchToWindowCommand
        {
            get { return this._switchToWindowCommand; }
            set
            {
                if (Equals(value, this._switchToWindowCommand)) return;
                this._switchToWindowCommand = value;
                this.OnPropertyChanged();
            }
        }

        public ICommand SwitchToFullScreenCommand
        {
            get { return this._switchToFullScreenCommand; }
            set
            {
                if (Equals(value, this._switchToFullScreenCommand)) return;
                this._switchToFullScreenCommand = value;
                this.OnPropertyChanged();
            }
        }

        public ICommand AddAllCommand { get; private set; }

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

        public ICommand ShowFilterGridCommand
        {
            get { return this._showFilterGridCommand; }
            set
            {
                if (Equals(value, this._showFilterGridCommand)) return;
                this._showFilterGridCommand = value;
                this.OnPropertyChanged();
            }
        }

        public Visibility FilterGridVisibility
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

        private void PlayAll()
        {
            this._eventAggregator.GetEvent<PlayAllEvent>().Publish(null);
        }

        private void AddRange()
        {
            this._eventAggregator.GetEvent<OnAddVideoRangeRequest>().Publish(null);
        }


        private void ShowFilterGrid()
        {
            if (this.FilterGridVisibility == Visibility.Collapsed)
            {
                this.FilterGridVisibility = Visibility.Visible;
            }
            else
            {
                this.FilterGridVisibility = Visibility.Collapsed;
            }
        }

        private void SwitchToWindowMode()
        {
            this.SwitchToWindowMode(null);
        }

        private void SwitchToWindowMode(object dummy)
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