using System;
using System.ComponentModel;

namespace VideoPlayer.ViewModels
{
    public class SortingViewModel:ViewModelBase
    {
        private String _name;
        private SortDescription _sortDescription;

        public SortDescription SortDescription
        {
            get { return this._sortDescription; }
            set
            {
                if (value.Equals(this._sortDescription)) return;
                this._sortDescription = value;
                this.OnPropertyChanged();
            }
        }

        public String Name
        {
            get { return this._name; }
            set
            {
                if (value == this._name) return;
                this._name = value;
                this.OnPropertyChanged();
            }
        }
    }
}