using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Classes;
using LeDragon.Log.Standard;
using Microsoft.Practices.Prism;
using Microsoft.Practices.Prism.PubSubEvents;
using Module.Interfaces;
using VideoPlayer.Infrastructure;
using VideoPlayer.Services;

namespace Module
{
    public class CategoryListViewModel : VideoPlayer.Infrastructure.ViewFirst.ViewModelBase, ICategoryListViewModel
    {
        private readonly ICategoryService _categoryService;
        private readonly IEventAggregator _eventAggregator;

        private ObservableCollection<CategoryViewModel> _categoryViewModels;
        private ICommand _filterByCategoryCommand;
        private CategoryViewModel _selectedCategory;

        public CategoryListViewModel(ICategoryService categoryService, IEventAggregator eventAggregator)
            
        {
            //this._libraryService = libraryService;
            this._categoryService = categoryService;
            this._eventAggregator = eventAggregator;
            this.InitCollection();
            this._eventAggregator.GetEvent<VideoEdited>().Subscribe(this.Refresh);
            this._eventAggregator.GetEvent<LibraryUpdated>().Subscribe(this.BuildCategoryList);
        }

        public ICommand FilterByCategoryCommand
        {
            get { return this._filterByCategoryCommand; }
            set
            {
                if (Equals(value, this._filterByCategoryCommand))
                {
                    return;
                }
                this._filterByCategoryCommand = value;
                this.OnPropertyChanged();
            }
        }

        public ObservableCollection<CategoryViewModel> CategoryViewModels
        {
            get { return this._categoryViewModels; }
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
                if (this._selectedCategory != null)
                {
                    this._eventAggregator.GetEvent<SelectedCategoryChangedEvent>().Publish(this.SelectedCategory.Name);
                }
                this.OnPropertyChanged();
            }
        }

        private void Refresh(Object dummy)
        {
            this.BuildCategoryList();
        }

        private void InitCollection()
        {
            try
            {
                if (this.CategoryViewModels == null)
                {
                    this.CategoryViewModels = new ObservableCollection<CategoryViewModel>();
                }

                this.BuildCategoryList();

                this.SelectedCategory = this.CategoryViewModels.First();
            }
            catch (Exception e)
            {
                this.Logger().ErrorFormat(e.Message);
            }
        }

        private void BuildCategoryList(IEnumerable<Video> videos = null)
        {
            var selectedCategoryName = this.SelectedCategory == null ? String.Empty : this.SelectedCategory.Name;

            this.CategoryViewModels.Clear();
            var categoryViewModels = this._categoryService.GetCategories()
                .Select(g => new CategoryViewModel
                {
                    Count = g.Value,
                    Name = g.Key
                });
            this.CategoryViewModels.AddRange(categoryViewModels);

            this.SelectedCategory = this.CategoryViewModels.SingleOrDefault(c => c.Name == selectedCategoryName) ??
                                    this.CategoryViewModels.First();
        }
    }
}