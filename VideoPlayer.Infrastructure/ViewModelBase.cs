using System.ComponentModel;
using System.Runtime.CompilerServices;
using VideoPlayer.Infrastructure.Annotations;

namespace VideoPlayer.Infrastructure
{
    public abstract class ViewModelBase : INotifyPropertyChanged, IViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ViewModelBase(IView view)
        {
            this.View = view;
            this.View.ViewModel = this;
        }

        public IView View { get; set; }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}