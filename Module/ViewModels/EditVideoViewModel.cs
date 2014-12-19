using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Classes;
using Microsoft.Practices.Unity;
using VideoPlayer.Common;
using VideoPlayer.Services;

namespace Module
{
    public class EditVideoViewModel : Video
    {
        private ObservableCollection<CategoryViewModel> _categoryViewModels;
        private CategoryViewModel _selectedCategory;

        public EditVideoViewModel()
        {
            var service = Locator.Container.Resolve<ILibraryService>();
            this.CategoryViewModels = new ObservableCollection<CategoryViewModel>();
            IEnumerable<IGrouping<string, Video>> categories =
                service.GetObjectsFromFile()
                    .Videos.Where(v => !String.IsNullOrEmpty(v.Category))
                    .GroupBy(v => v.Category);
            foreach (var category in categories)
            {
                this.CategoryViewModels.Add(new CategoryViewModel
                {
                    Name = category.Key,
                    Count = category.Count()
                });
            }
        }

        public CategoryViewModel SelectedCategory
        {
            get
            {
                CategoryViewModel vm = this.CategoryViewModels.FirstOrDefault(c => c.Name == this.Category) ??
                                       this.CategoryViewModels.First();
                return vm;
            }
            set
            {
                if (Equals(value, this._selectedCategory)) return;
                this._selectedCategory = value;
                //this.Category = this._selectedCategory.Name;
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
    }
}