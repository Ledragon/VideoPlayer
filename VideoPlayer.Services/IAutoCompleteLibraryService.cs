using System;
using System.Threading.Tasks;

namespace VideoPlayer.Services
{
    public interface IAutoCompleteLibraryService
    {
        Task AutoCompleteLibrary(IProgress<String> progress);
    }
}