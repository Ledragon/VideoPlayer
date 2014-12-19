using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
//using System.ComponentModel;
using System.Linq;
using Classes;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.PubSubEvents;
using VideoPlayer.Infrastructure;
using VideoPlayer.Services;

namespace Module
{
    public class VideoFilterGridViewModel : ViewModelBase, IVideoFilterGridViewModel
    {
        //private readonly SortingViewModel _categorySortingViewModel = new SortingViewModel
        //{
        //    Name = "Category",
        //    SortDescription = new SortDescription("Category", ListSortDirection.Ascending)
        //};

        private readonly IEventAggregator _eventAggregator;

        //private readonly SortingViewModel _lengthSortingViewModel = new SortingViewModel
        //{
        //    Name = "Length",
        //    SortDescription = new SortDescription("Length", ListSortDirection.Ascending)
        //};

        private readonly ILibraryService _libraryService;

        //private readonly SortingViewModel _titleSortingViewModel = new SortingViewModel
        //{
        //    Name = "Title",
        //    SortDescription = new SortDescription("Title", ListSortDirection.Ascending)
        //};


        private DelegateCommand _addTagCommand;
        private string _nameFilter;
        //private SortingViewModel _selectedSorting;
        private CategoryViewModel _selectedTag;
        private CategoryViewModel _selectedTagToDelete;
        private ObservableCollection<CategoryViewModel> _selectedTags;
        //private ObservableCollection<SortingViewModel> _sortings;
        private ObservableCollection<CategoryViewModel> _tags;

        public VideoFilterGridViewModel(IVideoFilterGrid videoFilterGrid, IEventAggregator eventAggregator,
            ILibraryService libraryService)
            : base(videoFilterGrid)
        {
            this._eventAggregator = eventAggregator;
            this._libraryService = libraryService;
            this.SelectedTags = new ObservableCollection<CategoryViewModel>();
            this.Tags = new ObservableCollection<CategoryViewModel>();
            this.AddTagCommand = new DelegateCommand(this.AddTag);
            this.RemoveTagCommand = new DelegateCommand(this.RemoveTag);
            this.ClearTagsCommand = new DelegateCommand(this.ClearTags);
            //this.SortByLengthCommand = new DelegateCommand(this.SortByLength);
            //this.SortByTitleCommand = new DelegateCommand(this.SortByTitle);
            //this.SortByCategoryCommand = new DelegateCommand(this.SortByCategory);
            this.CreateTagsList(null);

            this._eventAggregator.GetEvent<VideoEdited>().Subscribe(this.CreateTagsList);
        }

        public DelegateCommand SortByTitleCommand { get; set; }
        public DelegateCommand SortByCategoryCommand { get; set; }
        public DelegateCommand RemoveTagCommand { get; set; }
        public DelegateCommand ClearTagsCommand { get; set; }
        public DelegateCommand SortByLengthCommand { get; set; }

        public CategoryViewModel SelectedTagToDelete
        {
            get { return this._selectedTagToDelete; }
            set
            {
                if (Equals(value, this._selectedTagToDelete)) return;
                this._selectedTagToDelete = value;
                this.OnPropertyChanged();
            }
        }

        public DelegateCommand AddTagCommand
        {
            get { return this._addTagCommand; }
            set
            {
                if (Equals(value, this._addTagCommand)) return;
                this._addTagCommand = value;
                this.OnPropertyChanged();
            }
        }

        public CategoryViewModel SelectedTag
        {
            get { return this._selectedTag; }
            set
            {
                if (Equals(value, this._selectedTag)) return;
                this._selectedTag = value;
                this.OnPropertyChanged();
            }
        }

        //public SortingViewModel SelectedSorting
        //{
        //    get { return this._selectedSorting; }
        //    set
        //    {
        //        if (Equals(value, this._selectedSorting)) return;
        //        this._selectedSorting = value;
        //        this._eventAggregator.GetEvent<SortingChangedEvent>().Publish(this._selectedSorting.SortDescription);
        //        this.OnPropertyChanged();
        //    }
        //}

        //public ObservableCollection<SortingViewModel> Sortings
        //{
        //    get
        //    {
        //        if (this._sortings == null)
        //        {
        //            this._sortings = new ObservableCollection<SortingViewModel>
        //            {
        //                this._titleSortingViewModel,
        //                this._categorySortingViewModel,
        //                this._lengthSortingViewModel,
        //                new SortingViewModel
        //                {
        //                    Name = "Oldest",
        //                    SortDescription = new SortDescription("DateAdded", ListSortDirection.Ascending)
        //                },
        //                new SortingViewModel
        //                {
        //                    Name = "Newest",
        //                    SortDescription = new SortDescription("DateAdded", ListSortDirection.Descending)
        //                },
        //                new SortingViewModel
        //                {
        //                    Name = "Most viewed",
        //                    SortDescription = new SortDescription("NumberOfViews", ListSortDirection.Descending)
        //                },
        //                new SortingViewModel
        //                {
        //                    Name = "Least viewed",
        //                    SortDescription = new SortDescription("NumberOfViews", ListSortDirection.Ascending)
        //                }
        //            };
        //            this.SelectedSorting = this._sortings[0];
        //        }
        //        return this._sortings;
        //    }
        //    set
        //    {
        //        if (Equals(value, this._sortings)) return;
        //        this._sortings = value;
        //        this.OnPropertyChanged();
        //    }
        //}

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

        public ObservableCollection<CategoryViewModel> Tags
        {
            get { return this._tags; }
            set
            {
                if (Equals(value, this._tags)) return;
                this._tags = value;
                this.OnPropertyChanged();
            }
        }

        public ObservableCollection<CategoryViewModel> SelectedTags
        {
            get { return this._selectedTags; }
            set
            {
                if (Equals(value, this._selectedTags)) return;
                this._selectedTags = value;
                this.OnPropertyChanged();
            }
        }

        //private void SortByLength()
        //{
        //    this.SelectedSorting = this._lengthSortingViewModel;
        //}

        //private void SortByTitle()
        //{
        //    this.SelectedSorting = this._titleSortingViewModel;
        //}

        //private void SortByCategory()
        //{
        //    this.SelectedSorting = this._categorySortingViewModel;
        //}

        private void CreateTagsList(Object dummy)
        {
            this.Tags.Clear();
            ObservableCollection<Video> videos = this._libraryService.GetObjectsFromFile().Videos;
            IEnumerable<string> tags =
                videos.SelectMany(v => v.Tags).Select(t => t.Value).Distinct().OrderBy(t => t);
            foreach (string tag in tags)
            {
                this.Tags.Add(new CategoryViewModel
                {
                    Name = tag,
                    Count = videos.Count(v => v.Tags.Any(t => t.Value == tag))
                });
            }
        }

        private void ClearTags()
        {
            this.SelectedTags.Clear();
            this._eventAggregator.GetEvent<TagFilterChangedEvent>().Publish(new List<String>());
        }

        private void RemoveTag()
        {
            //this.SelectedTags.Clear();
            if (this.SelectedTagToDelete != null)
            {
                this.SelectedTags.Remove(this.SelectedTagToDelete);
                this._eventAggregator.GetEvent<TagFilterChangedEvent>()
                    .Publish(this.SelectedTags.Select(t => t.Name).ToList());
            }
        }

        private void AddTag()
        {
            if (this.SelectedTag != null && !this.SelectedTags.Contains(this.SelectedTag))
            {
                this.SelectedTags.Add(this.SelectedTag);
                this._eventAggregator.GetEvent<TagFilterChangedEvent>()
                    .Publish(this.SelectedTags.Select(s => s.Name).ToList());
            }
        }
    }
}