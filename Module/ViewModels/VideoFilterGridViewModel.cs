using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Microsoft.Practices.Prism.PubSubEvents;
using VideoPlayer.Infrastructure;

namespace Module
{
    public class VideoFilterGridViewModel : ViewModelBase, IVideoFilterGridViewModel
    {
        private readonly IEventAggregator _eventAggregator;
        private string _nameFilter;
        private SortingViewModel _selectedSorting;
        private ObservableCollection<SortingViewModel> _sortings;
        private string _tagFilter;

        public VideoFilterGridViewModel(IVideoFilterGrid videoFilterGrid, IEventAggregator eventAggregator)
            : base(videoFilterGrid)
        {
            this._eventAggregator = eventAggregator;
        }

        public SortingViewModel SelectedSorting
        {
            get { return this._selectedSorting; }
            set
            {
                if (Equals(value, this._selectedSorting)) return;
                this._selectedSorting = value;
                this._eventAggregator.GetEvent<SortingChangedEvent>().Publish(this._selectedSorting.SortDescription);
                this.OnPropertyChanged();
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

        public String TagFilter
        {
            get { return this._tagFilter; }
            set
            {
                if (value == this._tagFilter) return;
                this._tagFilter = value;
                this._eventAggregator.GetEvent<TagFilterChangedEvent>().Publish(this._tagFilter);
                this.OnPropertyChanged();
            }
        }

        public String NameFilter
        {
            get { return this._nameFilter; }
            set
            {
                if (value == this._nameFilter) return;
                this._nameFilter = value;
                this._eventAggregator.GetEvent<NameFilterChangedEvent>().Publish(this._nameFilter);
                this.OnPropertyChanged();
            }
        }
    }
}