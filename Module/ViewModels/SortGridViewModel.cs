using System.ComponentModel;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.PubSubEvents;
using Module.Views;
using VideoPlayer.Infrastructure;

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

        private readonly SortingViewModel _lengthSortingViewModel = new SortingViewModel
        {
            Name = "Length",
            SortDescription = new SortDescription("Length", ListSortDirection.Ascending)
        };

        private readonly SortingViewModel _titleSortingViewModel = new SortingViewModel
        {
            Name = "Title",
            SortDescription = new SortDescription("Title", ListSortDirection.Ascending)
        };

        private SortingViewModel _selectedSorting;

        #endregion

        public SortGridViewModel(ISortGrid view, IEventAggregator eventAggregator) : base(view)
        {
            this._eventAggregator = eventAggregator;
            this.SortByLengthCommand = new DelegateCommand(this.SortByLength);
            this.SortByTitleCommand = new DelegateCommand(this.SortByTitle);
            this.SortByCategoryCommand = new DelegateCommand(this.SortByCategory);
        }

        public DelegateCommand SortByTitleCommand { get; set; }
        public DelegateCommand SortByCategoryCommand { get; set; }
        public DelegateCommand SortByLengthCommand { get; set; }

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
    }
}