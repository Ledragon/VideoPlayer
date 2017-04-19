using System;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using VideoPlayer.Views;

namespace VideoPlayer.ViewModels
{
    public class PlayListManagementViewModel : Infrastructure.ViewModelBase, IPlayListManagementViewModel
    {
        private Boolean _filterGridVisibility;
        private Boolean _isCategoryGridVisible;
        private Boolean _isPlayListVisible;

        public PlayListManagementViewModel(IPlayListManagementView view) : base(view)
        {
            this.FilterGridVisibility = false;
            this.IsPlayListVisible = true;
            this.IsCategoryGridVisible = true;

            this.SwitchFilterGridVisibilityCommand = new DelegateCommand(this.SwitchFilterGridVisibility);
            this.SwitchPlaylistVisibilityCommand = new DelegateCommand(this.SwitchPlayListVisibility);
            this.SwitchCategoryGridVisibilityCommand =
                new DelegateCommand(() => this.IsCategoryGridVisible = !this.IsCategoryGridVisible);
        }

        public Boolean IsCategoryGridVisible
        {
            get { return this._isCategoryGridVisible; }
            set
            {
                if (value == this._isCategoryGridVisible)
                {
                    return;
                }
                this._isCategoryGridVisible = value;
                this.OnPropertyChanged();
            }
        }

        public Boolean IsPlayListVisible
        {
            get { return this._isPlayListVisible; }
            set
            {
                if (value == this._isPlayListVisible)
                {
                    return;
                }
                this._isPlayListVisible = value;
                this.OnPropertyChanged();
            }
        }

        public Boolean FilterGridVisibility
        {
            get { return this._filterGridVisibility; }
            set
            {
                if (!value.Equals(this._filterGridVisibility))
                {
                    this._filterGridVisibility = value;
                    this.OnPropertyChanged();
                }
            }
        }

        public ICommand SwitchCategoryGridVisibilityCommand { get; }
        public ICommand SwitchPlaylistVisibilityCommand { get; }
        public ICommand SwitchFilterGridVisibilityCommand { get; }


        private void SwitchPlayListVisibility()
        {
            this.IsPlayListVisible = !this.IsPlayListVisible;
        }

        private void SwitchFilterGridVisibility()
        {
            this.FilterGridVisibility = !this.FilterGridVisibility;
        }
    }
}