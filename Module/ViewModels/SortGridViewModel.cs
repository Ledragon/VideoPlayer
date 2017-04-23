using System.ComponentModel;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.PubSubEvents;
using VideoPlayer.Infrastructure;
using ViewModelBase = VideoPlayer.Infrastructure.ViewFirst.ViewModelBase;

namespace Module
{
    public class SortGridViewModel : ViewModelBase, ISortGridViewModel
    {
        #region Private members

        private readonly SortingViewModel _categorySortingViewModel = new SortingViewModel
        {
            Name = "Category",
            SortDescription = new SortDescription("Category", ListSortDirection.Ascending)
        };

        private readonly IEventAggregator _eventAggregator;

        private readonly SortingViewModel _leastViewedViewModel = new SortingViewModel
        {
            Name = "Least viewed",
            SortDescription = new SortDescription("NumberOfViews", ListSortDirection.Ascending)
        };

        private readonly SortingViewModel _lengthSortingViewModel = new SortingViewModel
        {
            Name = "Length",
            SortDescription = new SortDescription("Length", ListSortDirection.Ascending)
        };

        private readonly SortingViewModel _mostViewedViewModel = new SortingViewModel
        {
            Name = "Most viewed",
            SortDescription = new SortDescription("NumberOfViews", ListSortDirection.Descending)
        };

        private readonly SortingViewModel _newestSortingViewModel = new SortingViewModel
        {
            Name = "Newest",
            SortDescription = new SortDescription("DateAdded", ListSortDirection.Descending)
        };

        private readonly SortingViewModel _oldestSortingViewModel = new SortingViewModel
        {
            Name = "Oldest",
            SortDescription = new SortDescription("DateAdded", ListSortDirection.Ascending)
        };

        private readonly SortingViewModel _titleSortingViewModel = new SortingViewModel
        {
            Name = "Title",
            SortDescription = new SortDescription("Title", ListSortDirection.Ascending)
        };

        private SortingViewModel _selectedSorting;

        #endregion

        public SortGridViewModel(IEventAggregator eventAggregator)
        {
            this._eventAggregator = eventAggregator;
            this.SortByLengthCommand = new DelegateCommand(this.SortByLength);
            this.SortByTitleCommand = new DelegateCommand(this.SortByTitle);
            this.SortByCategoryCommand = new DelegateCommand(this.SortByCategory);
            this.SortByMostViewedCommand = new DelegateCommand(this.SortByMostViewed);
            this.SortByLeastViewedCommand = new DelegateCommand(this.SortByLeastViewed);
            this.SortByOldestCommand = new DelegateCommand(this.SortByOldest);
            this.SortByNewestCommand = new DelegateCommand(this.SortByNewest);
        }

        public DelegateCommand SortByTitleCommand { get; set; }
        public DelegateCommand SortByCategoryCommand { get; set; }
        public DelegateCommand SortByLengthCommand { get; set; }
        public DelegateCommand SortByMostViewedCommand { get; set; }
        public DelegateCommand SortByLeastViewedCommand { get; set; }
        public DelegateCommand SortByOldestCommand { get; set; }
        public DelegateCommand SortByNewestCommand { get; set; }

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

        private void SortByNewest()
        {
            this.SelectedSorting = this._newestSortingViewModel;
        }

        private void SortByOldest()
        {
            this.SelectedSorting = this._oldestSortingViewModel;
        }

        private void SortByLength()
        {
            this.SelectedSorting = this._lengthSortingViewModel;
        }

        private void SortByTitle()
        {
            this.SelectedSorting = this._titleSortingViewModel;
        }

        private void SortByCategory()
        {
            this.SelectedSorting = this._categorySortingViewModel;
        }

        private void SortByMostViewed()
        {
            this.SelectedSorting = this._mostViewedViewModel;
        }

        private void SortByLeastViewed()
        {
            this.SelectedSorting = this._leastViewedViewModel;
        }
    }
}