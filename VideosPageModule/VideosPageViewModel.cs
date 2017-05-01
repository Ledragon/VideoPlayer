using System;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.PubSubEvents;
using VideoPlayer.Infrastructure;
using ViewModelBase = VideoPlayer.Infrastructure.ViewFirst.ViewModelBase;

namespace VideosPageModule
{
    public class VideosPageViewModel : ViewModelBase, IVideosPageViewModel
    {
        private Cursor _cursor;
        private Int32 _selectedIndex;

        public VideosPageViewModel(IEventAggregator eventAggregator)
        {
            this.InitCommands();
            eventAggregator.GetEvent<OnStop>().Subscribe(this.SwitchToWindowMode);
            eventAggregator.GetEvent<PlayOneEvent>()
                .Subscribe(video => this.SelectedIndex = 1);
            eventAggregator.GetEvent<OnPlayPlaylistRequest>()
                .Subscribe(video => this.SelectedIndex = 1);
            eventAggregator.GetEvent<PlayCompleted>()
                .Subscribe(dummy => this.SelectedIndex = 0);

        }

        public ICommand SwitchToWindowCommand { get; private set; }
        public ICommand SwitchToFullScreenCommand { get; private set; }

        public Cursor Cursor
        {
            get { return this._cursor; }
            set
            {
                if (Equals(value, this._cursor))
                {
                    return;
                }
                this._cursor = value;
                this.OnPropertyChanged();
            }
        }

        public Int32 SelectedIndex
        {
            get { return this._selectedIndex; }
            set
            {
                if (value == this._selectedIndex)
                {
                    return;
                }
                this._selectedIndex = value;
                this.OnPropertyChanged();
            }
        }

        private void InitCommands()
        {
            this.SwitchToFullScreenCommand = new DelegateCommand(this.SwitchToFullScreen);
            this.SwitchToWindowCommand = new DelegateCommand(this.SwitchToWindowMode);
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