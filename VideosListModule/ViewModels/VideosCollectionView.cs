using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using Classes;
using Classes.Annotations;
using Log;
using Microsoft.Practices.Prism;

namespace VideosListModule
{
    public sealed class VideosCollectionView : ListCollectionView
    {
        private readonly ObservableCollection<Video> _collection;
        private readonly Int32 _pageSize = 7;
        private String _categoryFilter;
        private String _filterName;
        private List<String> _filterTags;

        private Int32 _pageNumber;
        private SortDescription _sortDescription;

        public VideosCollectionView([NotNull] ObservableCollection<Video> collection) : base(collection)
        {
            this._collection = collection;
            this._pageNumber = 0;
            this.Filter = this.GetFilterPredicate();
            this._categoryFilter = "All";
            this._sortDescription = new SortDescription("Title", ListSortDirection.Ascending);
            this.SortDescriptions.Add(this._sortDescription);
            this.RefreshView();
            if (collection.Any())
            {
                this.MoveCurrentToFirst();
            }
        }

        // Overriding count allows for paging.
        public override Int32 Count
        {
            get { return Math.Min(this._pageSize, this.CurrentItems.Length); }
        }

        private Video[] CurrentItems
        {
            get
            {
                return this.Cast<Video>()
                    .Skip(this._pageNumber*this._pageSize)
                    .Take(this._pageSize)
                    .ToArray();
            }
        }

        public Boolean CanMoveToNextPage
        {
            get
            {
                return this.Cast<Video>().Count() > this._pageSize*(this._pageNumber + 1);
                ;
            }
        }

        public Boolean CanMoveToPreviousPage
        {
            get { return this._pageNumber > 0; }
        }

        public override Object GetItemAt(Int32 index)
        {
            var currentItems = this.CurrentItems;
            if (index >= currentItems.Count())
            {
                throw new IndexOutOfRangeException();
            }
            return currentItems[index];
        }

        public void Sort(SortDescription obj)
        {
            if (this._sortDescription != obj)
            {
                this._sortDescription = obj;
                this.SortDescriptions.Clear();
                this.SortDescriptions.Add(obj);
                this.SortDescriptions.Add(new SortDescription("Title", ListSortDirection.Ascending));
            }
        }

        public void FilterTag(List<String> tags)
        {
            this._filterTags = tags;
            this.RefreshView();
        }

        public void FilterName(String obj)
        {
            if (this._filterName != obj)
            {
                this._filterName = obj;
                this.RefreshView();
            }
        }

        public void FilterCategory(String category)
        {
            if (this._categoryFilter != category)
            {
                this._categoryFilter = category;
                this.RefreshView();
            }
        }

        public void NextPage()
        {
            if (this.CanMoveToNextPage)
            {
                this._pageNumber++;
                this.RefreshView(false);
            }
        }

        public void PreviousPage()
        {
            if (this.CanMoveToPreviousPage)
            {
                this._pageNumber--;
                this.RefreshView(false);
            }
        }

        private void RefreshView(Boolean reset = true)
        {
            if (reset)
            {
                this._pageNumber = 0;
            }
            this.Refresh();
            this.MoveCurrentToFirst();
        }

        private Predicate<Object> GetFilterPredicate()
        {
            return item =>
            {
                var result = true;
                try
                {
                    var video = item as Video;
                    var isNameOk = this.IsNameOk(video);
                    var isCategoryOk = this.IsCategoryOk(video, this._categoryFilter);
                    var isTagOk = this.IsTagOk(video);
                    result = isNameOk && isCategoryOk && isTagOk;
                }
                catch (Exception e)
                {
                    this.Logger().ErrorFormat(e.Message);
                }
                return result;
            };
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
                isCategoryOk = !String.IsNullOrEmpty(video.Category) &&
                               String.Equals(video.Category, category, StringComparison.InvariantCultureIgnoreCase);
            }
            return isCategoryOk;
        }

        private Boolean IsNameOk(Video video)
        {
            var result = true;
            if (!String.IsNullOrEmpty(this._filterName))
            {
                result = video.Title.ToLower().Contains(this._filterName.ToLower());
            }
            return result;
        }

        private Boolean IsTagOk(Video video)
        {
            var result = true;
            if (this._filterTags != null && this._filterTags.Any())
            {
                result =
                    this._filterTags.Any(
                        filterTag =>
                            video.Tags.Any(
                                t => String.Equals(t.Value, filterTag, StringComparison.CurrentCultureIgnoreCase)));
            }
            return result;
        }

        public void Update(IEnumerable<Video> videos)
        {
            this._collection.Clear();
            this._collection.AddRange(videos);
        }

        public FilterParameters GetFilter()
        {
            return new FilterParameters(this._filterName, this._categoryFilter, this._filterTags, this._sortDescription);
        }
    }

    public class FilterParameters
    {
        public FilterParameters(String name, String category, List<String> tags, SortDescription sortDescription)
        {
            this.Name = name;
            this.Category = category;
            this.Tags = tags;
            this.SortDescription = sortDescription;
        }

        public String Name { get; }
        public String Category { get; }
        public List<String> Tags { get; }
        public SortDescription SortDescription { get; }
    }
}