using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using Classes;
using VideoPlayer.Annotations;

namespace VideoPlayer.ViewModels
{
    public class VideosPageViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Video> _videoCollection;

        public ObservableCollection<Video> VideoCollection
        {
            get { return this._videoCollection; }
            set
            {
                if (!Equals(value, this._videoCollection))
                {
                    this._videoCollection = value;
                    this.OnPropertyChanged("VideoCollection");
                }
            }
        }

        public ICollectionView Categories
        {
            get
            {
                IEnumerable<string> categoryList = this._videoCollection.Select(v => v.Category).Distinct();
                List<CategoryListViewModel> listViewModels = categoryList.Select(category => new CategoryListViewModel
                {
                    Count = this._videoCollection.Count(video => video.Category == category),
                    Name = category
                }).ToList();
                return CollectionViewSource.GetDefaultView(listViewModels);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}