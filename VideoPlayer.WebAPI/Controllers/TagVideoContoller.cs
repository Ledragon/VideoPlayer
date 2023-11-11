using Microsoft.AspNetCore.Mvc;
using VideoPlayer.Database.Repository.Contracts;
using VideoPlayer.Entities;

namespace VideoPlayer.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagVideoController : ControllerBase
    {
        private readonly ITagVideoRepository _tagVideoRepository;

        public TagVideoController(ITagVideoRepository tagVideoRepository)
        {
            this._tagVideoRepository = tagVideoRepository;
        }

        [HttpGet]
        public async Task<List<TagVideo>> Get()
        {
            return await this._tagVideoRepository.GetAsync();
        }

        [HttpPost("add")]
        public async Task Add(List<TagVideo> tagVideos)
        {
            var existing = await this._tagVideoRepository.GetAsync();
            // Filter duplicates
            var toAdd = tagVideos.Where(tv => !existing.Any(e => (e.VideoId == tv.VideoId && e.TagId == tv.TagId)));
            await this._tagVideoRepository.AddAsync(toAdd);
        }
    }
}
