using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Classes;
using Log;
using VideoPlayer.Commands;
using VideoPlayer.Common;
using VideoPlayer.Services;

namespace VideoPlayer.ViewModels
{
    public class CategoryListViewModel : ViewModelBase
    {
        #region Private members
        private ObservableCollection<CategoryViewModel> _categoryViewModels;
        private ICommand _filterByCategoryCommand;
        private CategoryViewModel _selectedCategory;

        #endregion

        public CategoryListViewModel()
        {
            this.CategoryViewModels = new ObservableCollection<CategoryViewModel>();
            this.InitCollection();
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
                this.FilterByCategory();
                this.OnPropertyChanged();
            }
        }


        private void InitCommands()
        {
            this._filterByCategoryCommand = new FilterByCategoryCommand(this.FilterByCategory);
        }

        private void FilterByCategory()
        {
            this.Logger().InfoFormat("Filtered on category {0}.", this._selectedCategory.Name);
        }

        private void InitCollection()
        {
            try
            {
                var libraryService = DependencyFactory.Resolve<ILibraryService>();
                ObjectsWrapper wrapper = libraryService.GetObjectsFromFile();
                var categories =
                    wrapper.Videos
                        .Select(v => new {Name = v.Category})
                        .Distinct()
                        .OrderBy(c => c.Name);
                foreach (var category in categories)
                {
                    this.CategoryViewModels.Add(new CategoryViewModel
                    {
                        Name = category.Name,
                        Count = wrapper.Videos.Count(v => v.Category == category.Name)
                    });
                }
            }
            catch (Exception e)
            {
                this.Logger().ErrorFormat(e.Message);
            }
        }

    }
}