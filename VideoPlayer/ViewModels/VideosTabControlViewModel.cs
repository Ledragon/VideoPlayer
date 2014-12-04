using System;

namespace VideoPlayer.ViewModels
{
    public class VideosTabControlViewModel:ViewModelBase
    {
        private Int32 _selectedIndex;

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
    }
}