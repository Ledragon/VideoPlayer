using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using VideoPlayer.Infrastructure.Annotations;

namespace Module
{
    public class SortingViewModel : INotifyPropertyChanged
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

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}