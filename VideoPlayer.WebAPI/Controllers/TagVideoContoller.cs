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
    }
}
