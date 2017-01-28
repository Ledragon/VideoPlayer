using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace VideosListModule.ViewModels
{
    public class FilterParameters
    {
        public FilterParameters(String name, String category, List<String> tags, SortDescription sortDescription)
        {
            this.Name = name;
            this.Category = category;
            this.Tags = tags;
            this.SortDescription = sortDescription;
        }

        public String Name { get; }
        public String Category { get; }
        public List<String> Tags { get; }
        public SortDescription SortDescription { get; }
    }
}