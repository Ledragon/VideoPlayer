using System.Collections.Generic;

namespace VideoPlayer.Services
{
    public interface ICategoryService
    {
        IDictionary<System.String, System.Int32> GetCategories();
    }
}