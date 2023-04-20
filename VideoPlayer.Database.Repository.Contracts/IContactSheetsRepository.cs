using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VideoPlayer.Entities;

namespace VideoPlayer.Database.Repository.Contracts
{
    public interface IContactSheetsRepository
    {
        Task<ContactSheet> Add(ContactSheet contactSheet);
        Task<ContactSheet> GetForVideo(Int32 videoId);
        Task<IDictionary<Int32, ContactSheet>> GetForVideos(List<Int32> videoIds);
        Task<ContactSheet> Update(ContactSheet contactSheet);
    }
}