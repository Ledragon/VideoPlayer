using Microsoft.AspNetCore.Mvc;
using VideoPlayer.Database.Repository.Contracts;
using VideoPlayer.Entities;

namespace VideoPlayer.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly ITagsRepository _tagsRepository;

        public TagsController(ITagsRepository tagsRepository)
        {
            this._tagsRepository = tagsRepository;
        }

        [HttpGet]
        public List<Tag> Get()
        {
            return this._tagsRepository.Get();
        }

        [HttpPost]
        public Tag Add([FromBody] Tag tag)
        {
            return this._tagsRepository.Add(tag);
        }

    }
}
