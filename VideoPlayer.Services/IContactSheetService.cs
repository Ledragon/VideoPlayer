using System.Threading.Tasks;
using VideoPlayer.Entities;

namespace VideoPlayer.Services
{
    public interface IContactSheetService
    {
        Task<ContactSheet> CreateContactSheet(System.Int32 videoId, System.Int32 nRows, System.Int32 nColumns);
    }
}