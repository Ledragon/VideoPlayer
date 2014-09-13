using System;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;
using Classes;
using Log;
using VideoPlayer.ViewModels;

namespace VideoPlayer.Views
{
    /// <summary>
    ///     Interaction logic for CategoryList.xaml
    /// </summary>
    public partial class CategoryList : UserControl
    {
        public CategoryList()
        {
            this.InitializeComponent();
        }

        private void CategoriesListboxItem_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
        }


        private void Filter(ICollectionView view, CategoryListViewModel categoryListViewModel)
        {
            if (view != null)
            {
                if (categoryListViewModel.Name == "All")
                {
                    view.Filter = this.TrueFilter;
                }
                else
                {
                    view.Filter = this.FilterCategory;
                }
            }
        }

        private Boolean TrueFilter(Object item)
        {
            return true;
        }

        private Boolean FilterCategory(Object item)
        {
            Boolean result = true;
            try
            {
                var video = item as Video;
                var categoryListViewModel = this.UiCategoriesListBox.SelectedItem as CategoryListViewModel;
                if (video != null && categoryListViewModel != null)
                {
                    String category = categoryListViewModel.Name;
                    Boolean isCategoryOk = this.IsCategoryOk(video, category);
                    result = isCategoryOk;
                }
            }
            catch (Exception e)
            {
                this.Logger().ErrorFormat(e.Message);
            }
            return result;
        }

        private Boolean IsCategoryOk(Video video, String category)
        {
            Boolean isCategoryOk = true;
            if (!String.IsNullOrEmpty(category) && !String.IsNullOrEmpty(video.Category))
            {
                isCategoryOk = video.Category == category;
            }
            return isCategoryOk;
        }
    }
}