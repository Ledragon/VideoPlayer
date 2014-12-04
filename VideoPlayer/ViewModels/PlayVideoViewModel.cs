using System.Windows.Input;
using System.Windows.Media;
using VideoPlayer.Commands;

namespace VideoPlayer.ViewModels
{
    public class PlayVideoViewModel : ViewModelBase
    {
        private ICommand _playCommand;
        private ImageSource _source;

        public PlayVideoViewModel()
        {
            this._playCommand = new PlayCommand(this.Play);
        }

        public ICommand PlayCommand
        {
            get { return this._playCommand; }
            set
            {
                if (Equals(value, this._playCommand)) return;
                this._playCommand = value;
                this.OnPropertyChanged();
            }
        }

        public ImageSource Source
        {
            get { return this._source; }
            set
            {
                if (!Equals(value, this._source))
                {
                    this._source = value;
                    this.OnPropertyChanged();
                }
            }
        }

        private void Play()
        {
            //TODO
        }
    }
}