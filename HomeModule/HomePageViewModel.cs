﻿using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.PubSubEvents;
using VideoPlayer.Infrastructure;
using VideoPlayer.Services;

namespace HomeModule
{
    public class HomePageViewModel : ViewModelBase, IHomePageViewModel
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly ILibraryService _libraryService;

        public HomePageViewModel(IHomePage homePage, IEventAggregator eventAggregator, ILibraryService libraryService)
            : base(homePage)
        {
            this._eventAggregator = eventAggregator;
            this._libraryService = libraryService;
            this.GoToSettingsCommand = new DelegateCommand(this.GoToSettings);
            this.GoToVideosCommand = new DelegateCommand(this.GoToVideos);
            this.CleanCommand = new DelegateCommand(this.CleanAsync);
            this.LoadCommand = new DelegateCommand(this.LoadAsync);
        }

        public ICommand GoToSettingsCommand { get; private set; }
        public ICommand GoToVideosCommand { get; private set; }
        public ICommand LoadCommand { get; private set; }
        public ICommand CleanCommand { get; private set; }

        private void Load()
        {
            this._eventAggregator.GetEvent<LibraryUpdating>().Publish(null);
            this._libraryService.Update();
            this._eventAggregator.GetEvent<LibraryUpdated>()
                .Publish(null);
        }

        private async void LoadAsync()
        {
            this._eventAggregator.GetEvent<LibraryUpdating>().Publish("Loading");
            await this._libraryService.UpdateAsync();
            this._eventAggregator.GetEvent<LibraryUpdated>()
                .Publish(null);
        }

        private void Clean()
        {
            this._eventAggregator.GetEvent<LibraryUpdating>().Publish("Cleaning");
            this._libraryService.Clean();
            this._eventAggregator.GetEvent<LibraryUpdated>().Publish(null);
        }

        private async void CleanAsync()
        {
            this._eventAggregator.GetEvent<LibraryUpdating>().Publish("Cleaning");
            await this._libraryService.CleanAsync();
            this._eventAggregator.GetEvent<LibraryUpdated>().Publish(null);
        }

        private void GoToSettings()
        {
            this._eventAggregator.GetEvent<GoToPage>().Publish(2);
        }

        private void GoToVideos()
        {
            this._eventAggregator.GetEvent<GoToPage>().Publish(1);
        }
    }
}