using System;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.PubSubEvents;
using VideoPlayer.Infrastructure;

namespace VideoPlayer.ViewModels
{
    public class VideosPageViewModel : Infrastructure.ViewModelBase, IVideosPageViewModel
    {
        private Cursor _cursor;
        private Int32 _selectedIndex;

        public VideosPageViewModel(IEventAggregator eventAggregator, IVideosPageView videosPageView)
            : base(videosPageView)
        {
            this.InitCommands();
            eventAggregator.GetEvent<OnStop>().Subscribe(this.SwitchToWindowMode);
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