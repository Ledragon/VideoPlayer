using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Practices.Prism.PubSubEvents;
using VideoPlayer.Infrastructure;
using VideoPlayer.Services;

namespace Module
{
    public class TagsListViewModel : ViewModelBase, ITagsListViewModel
    {
        private readonly IEventAggregator _eventAggregator;
        private CategoryViewModel _selectedTag;
        private ObservableCollection<CategoryViewModel> _selectedTags;
        private ObservableCollection<CategoryViewModel> _tags;

        public TagsListViewModel(ITagsListView view, ILibraryService libraryService, IEventAggregator eventAggregator)
            : base(view)
        {
            this._eventAggregator = eventAggregator;
            this.SelectedTags = new ObservableCollection<CategoryViewModel>();
            this.Tags = new ObservableCollection<CategoryViewModel>();
            var videos = libraryService.GetObjectsFromFile().Videos;
            IEnumerable<String> tags =
                videos.SelectMany(v => v.Tags).Select(t => t.Value).Distinct().OrderBy(t => t);
            foreach (var tag in tags)
            {
                this.Tags.Add(new CategoryViewModel
                {
                    Name = tag,
                    Count = videos.Count(v => v.Tags.Any(t => t.Value == tag))
                });
            }
        }

        public CategoryViewModel SelectedTag
        {
            get { return this._selectedTag; }
            set
            {
                if (Equals(value, this._selectedTag))
                {
                    return;
                }
                this._selectedTag = value;
                this.OnPropertyChanged();
            }
        }

        public ObservableCollection<CategoryViewModel> Tags
        {
            get { return this._tags; }
            set
            {
                if (Equals(value, this._tags))
                {
                    return;
                }
                this._tags = value;
                this.OnPropertyChanged();
            }
        }

        public ObservableCollection<CategoryViewModel> SelectedTags
        {
            get { return this._selectedTags; }
            set
            {
                if (Equals(value, this._selectedTags))
                {
                    return;
                }
                this._selectedTags = value;
                this.OnPropertyChanged();
            }
        }
    }
}