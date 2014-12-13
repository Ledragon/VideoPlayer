using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Classes;
using Log;
using Microsoft.Practices.Prism.PubSubEvents;
using VideoPlayer.Infrastructure;
using VideoPlayer.Services;

namespace Module
{
    public class CategoryListViewModel : ViewModelBase, ICategoryListViewModel
    {
        private readonly IEventAggregator _eventAggregator;

        #region Private members

        private ObservableCollection<CategoryViewModel> _categoryViewModels;
        private ICommand _filterByCategoryCommand;
        private CategoryViewModel _selectedCategory;

        #endregion

        public CategoryListViewModel(ILibraryService libraryService, ICategoryListView categoryListView, IEventAggregator eventAggregator)
            : base(categoryListView)
        {
            this._eventAggregator = eventAggregator;
            this.InitCollection(libraryService);
            this.InitCommands();
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
                this._eventAggregator.GetEvent<SelectedCategoryChangedEvent>().Publish(this.SelectedCategory.Name);
                this.FilterByCategory();
                this.OnPropertyChanged();
            }
        }

        private void InitCommands()
        {
            //this._filterByCategoryCommand = new FilterByCategoryCommand(this.FilterByCategory);
        }

        private void FilterByCategory()
        {
            //this.Logger().InfoFormat("Filtered on category {0}.", this._selectedCategory.Name);
        }

        private void InitCollection(ILibraryService libraryService)
        {
            try
            {
                ObjectsWrapper wrapper = libraryService.GetObjectsFromFile();
                IEnumerable<IGrouping<string, Video>> grouped =
                    wrapper.Videos.OrderBy(v => v.Category).GroupBy(v => v.Category);
                if (this.CategoryViewModels == null)
                {
                    this.CategoryViewModels = new ObservableCollection<CategoryViewModel>();
                }
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
                    Count = wrapper.Videos.Count,
                    Name = "All"
                });
                this.SelectedCategory = this.CategoryViewModels.First();
            }
            catch (Exception e)
            {
                this.Logger().ErrorFormat(e.Message);
            }
        }
    }
}