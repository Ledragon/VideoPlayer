using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using Classes;
using Log;
using VideoPlayer.Common;
using VideoPlayer.Services;

namespace VideoPlayer.ViewModels
{
    public class VideosTabControlViewModel : ViewModelBase
    {
        private ObservableCollection<CategoryViewModel> _categoryViewModels;
        private Video _currentVideo;
        private ICollectionView _filteredVideos;
        private CategoryViewModel _selectedCategory;
        private Int32 _selectedIndex;
        private ObservableCollection<Video> _videoCollection;

        public VideosTabControlViewModel()
        {
            var service = DependencyFactory.Resolve<ILibraryService>();
            this.VideoCollection = service.GetObjectsFromFile().Videos;
            this.CategoryViewModels = new ObservableCollection<CategoryViewModel>();
        }

        public CategoryViewModel SelectedCategory
        {
            get { return this._selectedCategory; }
            set
            {
                if (Equals(value, this._selectedCategory))
                {
                    return;
                }
                this._selectedCategory = value;
                this.Filter();
                this.OnPropertyChanged();
            }
        }

        public Video CurrentVideo
        {
            get { return this._currentVideo; }
            set
            {
                if (!Equals(value, this._currentVideo))
                {
                    this._currentVideo = value;
                    this.OnPropertyChanged();
                }
            }
        }

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

        public ObservableCollection<CategoryViewModel> CategoryViewModels
        {
            get
            {
                IEnumerable<IGrouping<string, Video>> grouped =
                    this.VideoCollection.OrderBy(v => v.Category).GroupBy(v => v.Category);
                foreach (var grouping in grouped)
                {
                    this._categoryViewModels.Add(new CategoryViewModel
                    {
                        Name = grouping.Key,
                        Count = grouping.Count()
                    });
                }


                this._categoryViewModels.Insert(0, new CategoryViewModel
                {
                    Count = this.VideoCollection.Count,
                    Name = "All"
                });
                this.SelectedCategory = this._categoryViewModels[0];
                return this._categoryViewModels;
            }
            set
            {
                if (Equals(value, this._categoryViewModels))
                {
                    return;
                }
                this._categoryViewModels = value;
                this.OnPropertyChanged();
            }
        }

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

        public ICollectionView FilteredVideos
        {
            get { return this._filteredVideos; }
            set
            {
                if (Equals(value, this._filteredVideos))
                {
                    return;
                }
                this._filteredVideos = value;
                this.OnPropertyChanged();
            }
        }

        private void Filter()
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(this.VideoCollection);
            if (view != null)
            {
                view.Filter = this.FilterCategory;
                this.FilteredVideos = view;
            }
        }

        private Boolean FilterCategory(Object item)
        {
            Boolean result = true;
            try
            {
                var video = item as Video;
                CategoryViewModel categoryListViewModel = this.SelectedCategory;
                if (video != null && categoryListViewModel != null)
                {
                    String category = categoryListViewModel.Name;
                    Boolean isCategoryOk = this.IsCategoryOk(video, category);
                    result = isCategoryOk;
                }
            }
            catch (Exception e)
            {
                this.Logger().ErrorFormat(e.Message);
            }
            return result;
        }

        private Boolean IsCategoryOk(Video video, String category)
        {
            Boolean isCategoryOk;
            if (String.Compare(category, "all", StringComparison.InvariantCultureIgnoreCase) == 0)
            {
                isCategoryOk = true;
            }
            else if (String.IsNullOrEmpty(category))
            {
                isCategoryOk = String.IsNullOrEmpty(video.Category);
            }
            else
            {
                isCategoryOk = !String.IsNullOrEmpty(video.Category) && video.Category == category;
            }
            return isCategoryOk;
        }
    }
}