using System;
using System.Collections.Generic;

namespace VideoPlayer.Services
{
    public interface ICategoryService
    {
        IDictionary<String, Int32> GetCategories();
    }
}