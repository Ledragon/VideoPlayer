using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using VideoPlayer.Infrastructure.Annotations;

namespace Module
{
    public class CategoryViewModel : INotifyPropertyChanged
    {
        private int _count;
        private string _name;

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

        public Int32 Count
        {
            get { return this._count; }
            set
            {
                if (value == this._count) return;
                this._count = value;
                this.OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}