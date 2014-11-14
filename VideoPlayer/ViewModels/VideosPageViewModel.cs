using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using Classes;

namespace VideoPlayer.ViewModels
{
    public class VideosPageViewModel : ViewModelBase
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
                    this.OnPropertyChanged();
                }
            }
        }

        public ICollectionView Categories
        {
            get
            {
                IEnumerable<string> categoryList = this._videoCollection.Select(v => v.Category).Distinct();
                List<CategoryViewModel> listViewModels = categoryList.Select(category => new CategoryViewModel
                {
                    Count = this._videoCollection.Count(video => video.Category == category),
                    Name = category
                }).ToList();
                return CollectionViewSource.GetDefaultView(listViewModels);
            }
        }
    }
}