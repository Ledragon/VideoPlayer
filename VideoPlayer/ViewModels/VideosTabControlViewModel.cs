using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Classes;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.PubSubEvents;
using VideoPlayer.Infrastructure;

namespace VideoPlayer.ViewModels
{
    public class VideosTabControlViewModel : ViewModelBase
    {
        private ICommand _clearFilterCommand;
        private Cursor _cursor;
        private Visibility _filterGridVisibility;
        private String _nameFilter;
        private int _numberOfVideos;
        private Int32 _selectedIndex;
        private SortingViewModel _selectedSorting;
        private ICommand _showFilterGridCommand;
        private ObservableCollection<SortingViewModel> _sortings;
        private ICommand _switchToFullScreenCommand;
        private ICommand _switchToWindowCommand;
        private string _tagFilter;

        public VideosTabControlViewModel(IEventAggregator eventAggregator)
        {
            this.FilterGridVisibility = Visibility.Collapsed;

            this.ShowFilterGridCommand = new DelegateCommand(this.ShowFilterGrid);
            this.SwitchToFullScreenCommand = new GenericCommand(this.SwitchToFullScreen);
            this.SwitchToWindowCommand = new GenericCommand(this.SwitchToWindowMode);
            this.ClearFilterCommand = new GenericCommand(this.ClearFilter);

            eventAggregator.GetEvent<PlayedEvent>().Subscribe(this.SwitchToFullScreen);
            eventAggregator.GetEvent<StoppedEvent>().Subscribe(this.SwitchToWindowMode);
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
                this.FilterTag,
                this.FilterName
            };

            return filters.All(predicate => predicate(item));
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
        }
    }
}