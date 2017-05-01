using System;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.PubSubEvents;
using VideoPlayer.Infrastructure;

namespace VideoPlayer.PlaylistManagement
{
    public class PlayListManagementViewModel : Infrastructure.ViewFirst.ViewModelBase, IPlayListManagementViewModel
    {
        private readonly IEventAggregator _eventAggregator;
        private Boolean _filterGridVisibility;
        private Boolean _isCategoryGridVisible;
        private Boolean _isPlayListVisible;

        public PlayListManagementViewModel(IEventAggregator eventAggregator)
        {
            this._eventAggregator = eventAggregator;
            this.FilterGridVisibility = false;
            this.IsPlayListVisible = true;
            this.IsCategoryGridVisible = true;

            this.SwitchFilterGridVisibilityCommand = new DelegateCommand(this.SwitchFilterGridVisibility);
            this.SwitchPlaylistVisibilityCommand = new DelegateCommand(this.SwitchPlayListVisibility);
            this.SwitchCategoryGridVisibilityCommand =
                new DelegateCommand(() => this.IsCategoryGridVisible = !this.IsCategoryGridVisible);
           
            this.PlayPlaylistCommand = new DelegateCommand(() =>
            {
                this._eventAggregator.GetEvent<OnPlayPlaylistRequest>()
                    .Publish(null);
            });
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
        public ICommand PlayPlaylistCommand { get; }

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