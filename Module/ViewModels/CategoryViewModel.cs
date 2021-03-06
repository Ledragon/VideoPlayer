﻿using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using VideoPlayer.Infrastructure.Annotations;

namespace Module
{
    public class CategoryViewModel : INotifyPropertyChanged
    {
        private Int32 _count;
        private String _name;

        public String Name
        {
            get { return this._name; }
            set
            {
                if (value == this._name)
                {
                    return;
                }
                this._name = value;
                this.OnPropertyChanged();
            }
        }

        public Int32 Count
        {
            get { return this._count; }
            set
            {
                if (value == this._count)
                {
                    return;
                }
                this._count = value;
                this.OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] String propertyName = null)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}