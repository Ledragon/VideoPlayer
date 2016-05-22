using System;
using System.Collections.Generic;
using System.Linq;

namespace VideoPlayer.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ILibraryService _libraryService;

        public CategoryService(ILibraryService libraryService)
        {
            this._libraryService = libraryService;
        }

        public IDictionary<String, Int32> GetCategories()
        {
            var wrapper = this._libraryService.GetObjectsFromFile();
            var videos = wrapper.Videos;
            var enumerable = videos.ToList();
            var grouped = enumerable
                .GroupBy(v => String.IsNullOrEmpty(v.Category) ? String.Empty : v.Category.ToLower())
                .Select(g => new {key = g.Key, value = g.Count()})
                .OrderBy(g => g.key)
                .ToList();
            grouped.Insert(0, new {key = "All", value = videos.Count});
            return grouped.ToDictionary(g => g.key, g => g.value);
        }
    }
}