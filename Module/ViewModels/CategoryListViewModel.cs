using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Classes;
using Log;
using Microsoft.Practices.Prism.PubSubEvents;
using Module.Interfaces;
using VideoPlayer.Infrastructure;
using VideoPlayer.Services;

namespace Module
{
    public class CategoryListViewModel : ViewModelBase, ICategoryListViewModel
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly ILibraryService _libraryService;

        public CategoryListViewModel(ILibraryService libraryService, ICategoryListView categoryListView,
            IEventAggregator eventAggregator)
            : base(categoryListView)
        {
            this._libraryService = libraryService;
            this._eventAggregator = eventAggregator;
            this.InitCollection(libraryService);
            this._eventAggregator.GetEvent<VideoEdited>().Subscribe(this.Refresh);
            this._eventAggregator.GetEvent<LibraryUpdated>().Subscribe(this.BuildCategoryList);
        }

        public ICommand FilterByCategoryCommand
        {
            get { return this._filterByCategoryCommand; }
            set
            {
                if (Equals(value, this._filterByCategoryCommand)) return;
                this._filterByCategoryCommand = value;
                this.OnPropertyChanged();
            }
        }

        public ObservableCollection<CategoryViewModel> CategoryViewModels
        {
            get { return this._categoryViewModels; }
            set
            {
                if (Equals(value, this._categoryViewModels)) return;
                this._categoryViewModels = value;
                this.OnPropertyChanged();
            }
        }

        public CategoryViewModel SelectedCategory
        {
            get { return this._selectedCategory; }
            set
            {
                if (Equals(value, this._selectedCategory)) return;
                this._selectedCategory = value;
                if (this._selectedCategory != null)
                {
                    this._eventAggregator.GetEvent<SelectedCategoryChangedEvent>().Publish(this.SelectedCategory.Name);
                }
                this.OnPropertyChanged();
            }
        }

        private void Refresh(Object dummy)
        {
            this.BuildCategorylist(this._libraryService);
        }

        private void InitCollection(ILibraryService libraryService)
        {
            try
            {
                if (this.CategoryViewModels == null)
                {
                    this.CategoryViewModels = new ObservableCollection<CategoryViewModel>();
                }

                this.BuildCategorylist(libraryService);

                this.SelectedCategory = this.CategoryViewModels.First();
            }
            catch (Exception e)
            {
                this.Logger().ErrorFormat(e.Message);
            }
        }

        private void BuildCategorylist(ILibraryService libraryService)
        {
            var wrapper = libraryService.GetObjectsFromFile();
            var videos = wrapper.Videos;
            this.BuildCategoryList(videos);
        }

        private void BuildCategoryList(IEnumerable<Video> videos)
        {
            var selectedCategoryName = this.SelectedCategory == null ? String.Empty : this.SelectedCategory.Name;

            this.CategoryViewModels.Clear();
            var enumerable = videos.ToList();
            var grouped =
                enumerable.OrderBy(v => v.Category).GroupBy(v => v.Category);
            foreach (var grouping in grouped)
            {
                this.CategoryViewModels.Add(new CategoryViewModel
                {
                    Name = grouping.Key,
                    Count = grouping.Count()
                });
            }
            this.CategoryViewModels.Insert(0, new CategoryViewModel
            {
                Count = enumerable.Count,
                Name = "All"
            });

            this.SelectedCategory = this.CategoryViewModels.SingleOrDefault(c => c.Name == selectedCategoryName) ??
                                    this.CategoryViewModels.First();
        }

        #region Private members

        private ObservableCollection<CategoryViewModel> _categoryViewModels;
        private ICommand _filterByCategoryCommand;
        private CategoryViewModel _selectedCategory;

        #endregion
    }
}