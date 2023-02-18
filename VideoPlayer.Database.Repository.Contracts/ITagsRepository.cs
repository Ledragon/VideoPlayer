using System;
using System.Collections.Generic;
using System.Text;
using VideoPlayer.Entities;

namespace VideoPlayer.Database.Repository.Contracts
{
    public interface ITagsRepository
    {
        List<Tag> Get();
        Tag Get(Int32 id);
        List<Tag> GetForVideo(Int32 videoId);
        Tag Add(Tag tag);
        List<Tag> Add(List<Tag> tags);
        Tag Delete(Int32 id);
    }
}
