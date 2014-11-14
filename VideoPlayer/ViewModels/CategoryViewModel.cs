using System;

namespace VideoPlayer.ViewModels
{
    public class CategoryViewModel : ViewModelBase
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
    }
}