using System.ComponentModel;
using System.Runtime.CompilerServices;
using VideoPlayer.Infrastructure.Annotations;

namespace VideoPlayer.Infrastructure.ViewFirst
{
    public abstract class ViewModelBase : INotifyPropertyChanged, IViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}