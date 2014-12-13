using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Classes;
using Log;
using Microsoft.Practices.Prism.PubSubEvents;
using VideoPlayer.Infrastructure;

namespace VideoPlayer.ViewModels
{
    public class VideosTabControlViewModel : ViewModelBase
    {
        private ObservableCollection<CategoryViewModel> _categoryViewModels;
        private ICommand _clearFilterCommand;
        private Cursor _cursor;
        private Visibility _filterGridVisibility;
        private String _nameFilter;
        private int _numberOfVideos;
        private CategoryViewModel _selectedCategory;
        private Int32 _selectedIndex;
        private SortingViewModel _selectedSorting;
        private ICommand _showFilterGridCommand;
        private ObservableCollection<SortingViewModel> _sortings;
        private ICommand _switchEditCommand;
        private ICommand _switchToFullScreenCommand;
        private ICommand _switchToWindowCommand;
        private string _tagFilter;
        private int _videoEditIndex;

        public VideosTabControlViewModel(IEventAggregator eventAggregator)
        {
            //var service = DependencyFactory.Resolve<ILibraryService>();
            //this.VideoCollection = service.GetObjectsFromFile().Videos;
            this.CategoryViewModels = new ObservableCollection<CategoryViewModel>();
            this.FilterGridVisibility = Visibility.Collapsed;

            this.ShowFilterGridCommand = new GenericCommand(this.ShowFilterGrid);
            this.SwitchToFullScreenCommand = new GenericCommand(this.SwitchToFullScreen);
            this.SwitchToWindowCommand = new GenericCommand(this.SwitchToWindowMode);
            this.ClearFilterCommand = new GenericCommand(this.ClearFilter);
            this.SwitchEditCommand = new GenericCommand(this.SwitchEdit);

            eventAggregator.GetEvent<PlayedEvent>().Subscribe(this.SwitchToFullScreen);
            eventAggregator.GetEvent<StoppedEvent>().Subscribe(this.SwitchToWindowMode);
        }

        public ICommand SwitchEditCommand
        {
            get { return this._switchEditCommand; }
            set
            {
                if (Equals(value, this._switchEditCommand)) return;
                this._switchEditCommand = value;
                this.OnPropertyChanged();
            }
        }

        public Int32 VideoEditIndex
        {
            get { return this._videoEditIndex; }
            set
            {
                if (value == this._videoEditIndex) return;
                this._videoEditIndex = value;
                this.OnPropertyChanged();
            }
        }

        public Int32 NumberOfVideos
        {
            get
            {
                return this._numberOfVideos;
                //return this.FilteredVideos.Cast<Video>().Count();
            }
            set
            {
                if (value == this._numberOfVideos) return;
                this._numberOfVideos = value;
                this.OnPropertyChanged();
            }
        }

        public ICommand ClearFilterCommand
        {
            get { return this._clearFilterCommand; }
            set
            {
                if (Equals(value, this._clearFilterCommand)) return;
                this._clearFilterCommand = value;
                this.OnPropertyChanged();
            }
        }

        public String TagFilter
        {
            get { return this._tagFilter; }
            set
            {
                if (value != this._tagFilter)
                {
                    this._tagFilter = value;
                    this.Filter();
                    this.OnPropertyChanged();
                }
            }
        }

        public ICommand SwitchToWindowCommand
        {
            get { return this._switchToWindowCommand; }
            set
            {
                if (Equals(value, this._switchToWindowCommand)) return;
                this._switchToWindowCommand = value;
                this.OnPropertyChanged();
            }
        }

        public ICommand SwitchToFullScreenCommand
        {
            get { return this._switchToFullScreenCommand; }
            set
            {
                if (Equals(value, this._switchToFullScreenCommand)) return;
                this._switchToFullScreenCommand = value;
                this.OnPropertyChanged();
            }
        }

        public Cursor Cursor
        {
            get { return this._cursor; }
            set
            {
                if (Equals(value, this._cursor)) return;
                this._cursor = value;
                this.OnPropertyChanged();
            }
        }

        public ICommand ShowFilterGridCommand
        {
            get { return this._showFilterGridCommand; }
            set
            {
                if (Equals(value, this._showFilterGridCommand)) return;
                this._showFilterGridCommand = value;
                this.OnPropertyChanged();
            }
        }

        public SortingViewModel SelectedSorting
        {
            get { return this._selectedSorting; }
            set
            {
                if (!Equals(value, this._selectedSorting))
                {
                    this._selectedSorting = value;
                    //this.FilteredVideos.SortDescriptions.Clear();
                    //this.FilteredVideos.SortDescriptions.Add(this._selectedSorting.SortDescription);
                    this.OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<SortingViewModel> Sortings
        {
            get
            {
                if (this._sortings == null)
                {
                    this._sortings = new ObservableCollection<SortingViewModel>
                    {
                        new SortingViewModel
                        {
                            Name = "Title",
                            SortDescription = new SortDescription("Title", ListSortDirection.Ascending)
                        },
                        new SortingViewModel
                        {
                            Name = "Category",
                            SortDescription = new SortDescription("Category", ListSortDirection.Ascending)
                        },
                        new SortingViewModel
                        {
                            Name = "Length",
                            SortDescription = new SortDescription("Length", ListSortDirection.Ascending)
                        },
                        new SortingViewModel
                        {
                            Name = "Oldest",
                            SortDescription = new SortDescription("DateAdded", ListSortDirection.Ascending)
                        },
                        new SortingViewModel
                        {
                            Name = "Newest",
                            SortDescription = new SortDescription("DateAdded", ListSortDirection.Descending)
                        }
                    };
                    this.SelectedSorting = this._sortings[0];
                }
                return this._sortings;
            }
            set
            {
                if (Equals(value, this._sortings)) return;
                this._sortings = value;
                this.OnPropertyChanged();
            }
        }

        public String NameFilter
        {
            get { return this._nameFilter; }
            set
            {
                if (value != this._nameFilter)
                {
                    this._nameFilter = value;
                    this.Filter();
                    this.OnPropertyChanged();
                }
            }
        }

        public Visibility FilterGridVisibility
        {
            get { return this._filterGridVisibility; }
            set
            {
                if (!value.Equals(this._filterGridVisibility))
                {
                    this._filterGridVisibility = value;
                    this.OnPropertyChanged();
                }
            }
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
                if (this._categoryViewModels.Count == 0)
                {
                    //IEnumerable<IGrouping<string, Video>> grouped =
                    //    this.VideoCollection.OrderBy(v => v.Category).GroupBy(v => v.Category);
                    //foreach (var grouping in grouped)
                    //{
                    //    this._categoryViewModels.Add(new CategoryViewModel
                    //    {
                    //        Name = grouping.Key,
                    //        Count = grouping.Count()
                    //    });
                    //}


                    //this._categoryViewModels.Insert(0, new CategoryViewModel
                    //{
                    //    Count = this.VideoCollection.Count,
                    //    Name = "All"
                    //});
                    //this.SelectedCategory = this._categoryViewModels[0];
                }
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

        private void SwitchEdit()
        {
            if (this.VideoEditIndex == 0)
            {
                this.VideoEditIndex = 1;
            }
            else
            {
                this.VideoEditIndex = 0;
                //this.FilteredVideos.Refresh();
            }
        }

        private void ClearFilter()
        {
            this.NameFilter = String.Empty;
            this.TagFilter = String.Empty;
        }

        private void ShowFilterGrid()
        {
            if (this.FilterGridVisibility == Visibility.Collapsed)
            {
                this.FilterGridVisibility = Visibility.Visible;
            }
            else
            {
                this.FilterGridVisibility = Visibility.Collapsed;
            }
        }

        private void Filter()
        {
            //ICollectionView view = CollectionViewSource.GetDefaultView(this.VideoCollection);
            //if (view != null)
            //{
            //    view.Filter = this.GlobalFilter;
            //    this.FilteredVideos = view;
            //    this.NumberOfVideos = this.FilteredVideos.Cast<Video>().Count();
            //}
        }

        private Boolean GlobalFilter(Object item)
        {
            var filters = new List<Predicate<Object>>
            {
                this.FilterCategory,
                this.FilterTag,
                this.FilterName
            };

            return filters.All(predicate => predicate(item));
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

        private Boolean FilterTag(Object item)
        {
            return this.IsTagOk(item as Video);
        }

        private Boolean FilterName(Object item)
        {
            return this.IsNameOk(item as Video);
        }

        private Boolean IsNameOk(Video video)
        {
            bool result = true;
            if (!String.IsNullOrEmpty(this.NameFilter))
            {
                result = video.Title.ToLower().Contains(this.NameFilter.ToLower());
            }
            return result;
        }

        private Boolean IsTagOk(Video video)
        {
            Boolean isTagOk = false;
            if (!String.IsNullOrEmpty(this.TagFilter))
            {
                String tagFilter = this.TagFilter;

                string[] filters = tagFilter.Split(new[] {";"}, StringSplitOptions.RemoveEmptyEntries);
                isTagOk = filters.Aggregate(isTagOk,
                    (current, filter) => current || video.Tags.Any(t => t.Value.ToLower().Contains(filter)));
            }
            else
            {
                isTagOk = true;
            }
            return isTagOk;
        }

        private void SwitchToWindowMode()
        {
            this.SwitchToWindowMode(null);
        }

        private void SwitchToWindowMode(object dummy)
        {
            if (this.SelectedIndex != 0)
            {
                this.SelectedIndex = 0;
                this.Cursor = Cursors.Arrow;
            }
        }

        private void SwitchToFullScreen()
        {
            this.SwitchToFullScreen(null);
        }

        private void SwitchToFullScreen(Object dummy)
        {
            if (this.SelectedIndex != 1)
            {
                this.SelectedIndex = 1;
            }
            //this._uiVideosTabControl.SelectedItem = this._uiVideoPlaying;
        }
    }
}