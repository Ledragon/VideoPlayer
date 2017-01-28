using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Data;
using Classes;
using Classes.Annotations;
using Log;

namespace VideosListModule
{
    public sealed class VideosCollectionView : ListCollectionView
    {
        private String _categoryFilter;
        private String _filterName;
        private List<String> _filterTags;

        private Int32 _pageNumber;
        private Int32 _pageSize = 20;
        private SortDescription _sortDescription;
        private readonly List<Video> _list;

        public VideosCollectionView([NotNull] List<Video> collection) : base(collection)
        {
            this._list = collection;
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

        public override Object GetItemAt(Int32 index)
        {
            var currentItems = this.CurrentItems;
            if (index >= currentItems.Count())
            {
                throw new IndexOutOfRangeException();
            }
            return currentItems[index];
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

        private void RefreshView()
        {
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
    }
}