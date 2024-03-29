﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Classes;
using LeDragon.Log.Standard;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.PubSubEvents;
using VideoPlayer.Entities;
using VideoPlayer.Infrastructure;
using VideoPlayer.Services;
using ViewModelBase = VideoPlayer.Infrastructure.ViewFirst.ViewModelBase;

namespace Module
{
    public class EditVideoViewModel : ViewModelBase, IEditVideoViewModel
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly ILibraryService _libraryService;
        private ObservableCollection<CategoryViewModel> _categoryViewModels;
        private String _newCategory;
        private CategoryViewModel _selectedCategory;
        private ObservableCollection<Tag> _tags;
        private Classes.VideoViewModel _video;


        public EditVideoViewModel(ILibraryService libraryService, IEventAggregator eventAggregator)
        {
            this._libraryService = libraryService;
            this._eventAggregator = eventAggregator;
            this.CategoryViewModels = new ObservableCollection<CategoryViewModel>();
            this.Tags = new ObservableCollection<Tag>();
            this.CreateCategories(libraryService);
            this.CreateTags(libraryService);
            eventAggregator.GetEvent<VideoEditing>().Subscribe(v => { this.Video = v; });
            //eventAggregator.GetEvent<VideoEdited>().Subscribe(this.VideoEdited);
            this.CreateCategoryCommand = new DelegateCommand(this.CreateCategory);
        }

        public ObservableCollection<Tag> Tags
        {
            get { return this._tags; }
            set
            {
                if (Equals(value, this._tags))
                {
                    return;
                }
                this._tags = value;
                this.OnPropertyChanged();
            }
        }

        public DelegateCommand CreateCategoryCommand { get; set; }

        public String NewCategory
        {
            get { return this._newCategory; }
            set
            {
                if (value == this._newCategory)
                {
                    return;
                }
                this._newCategory = value;
                this.OnPropertyChanged();
            }
        }

        public Classes.VideoViewModel Video
        {
            get { return this._video; }
            set
            {
                if (Equals(value, this._video))
                {
                    return;
                }
                this._video = value;
                this.SelectedCategory = this.CategoryViewModels.FirstOrDefault(c => c.Name == this.Video.Category);
                //if (value.PropertyChanged)
                //{
                value.PropertyChanged += (s, e) => { this._eventAggregator.GetEvent<VideoEdited>().Publish(value); };
                //}
                this.OnPropertyChanged();
            }
        }

        public CategoryViewModel SelectedCategory
        {
            get
            {
                return this._selectedCategory;
                //CategoryViewModel vm = this.CategoryViewModels.First();
                //if (this.Video != null && !String.IsNullOrEmpty(this.Video.Category))
                //{
                //    vm = this.CategoryViewModels.FirstOrDefault(c => c.Name == this.Video.Category);
                //}
                //return vm;
            }
            set
            {
                if (Equals(value, this._selectedCategory))
                {
                    return;
                }
                this._selectedCategory = value;
                if (this.Video != null && value != null)
                {
                    this.Video.Category = value.Name;
                }
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

        private void CreateTags(ILibraryService libraryService)
        {
            this.Tags.Clear();
            var videos = libraryService.GetObjectsFromFile()
                .Videos;
            IEnumerable<Tag> tags = videos
                .SelectMany(v => v.Tags)
                .Distinct()
                .OrderBy(t => t.Value);
            foreach (var tag in tags)
            {
                this.Tags.Add(tag);
            }
        }

        private void VideoEdited(Object obj)
        {
            this.CreateCategories(this._libraryService);
        }

        private void CreateCategories(ILibraryService libraryService)
        {
            this.CategoryViewModels.Clear();
            IEnumerable<IGrouping<String, VideoPlayer.Entities.Video>> categories =
                libraryService.GetObjectsFromFile()
                    .Videos.Where(v => !String.IsNullOrEmpty(v.Category))
                    .GroupBy(v => v.Category)
                    .OrderBy(g => g.Key);
            foreach (var category in categories)
            {
                this.CategoryViewModels.Add(new CategoryViewModel
                {
                    Name = category.Key,
                    Count = category.Count()
                });
            }
            if (this.CategoryViewModels.Any())
            {
                var vm = this.CategoryViewModels.First();
                if (this.Video != null && !String.IsNullOrEmpty(this.Video.Category))
                {
                    vm = this.CategoryViewModels.FirstOrDefault(c => c.Name == this.Video.Category);
                }
                this.SelectedCategory = vm;
            }
        }

        private void CreateCategory()
        {
            try
            {
                this.Video.Category = this.NewCategory;
                //this._eventAggregator.GetEvent<VideoEdited>().Publish(this.Video);
                this.VideoEdited(this.Video);
                this.NewCategory = String.Empty;
            }
            catch (Exception e)
            {
                this.Logger().Error(e.Message);
            }
        }
    }
}