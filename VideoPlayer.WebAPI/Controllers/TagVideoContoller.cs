using Microsoft.AspNetCore.Mvc;
using VideoPlayer.Database.Repository.Contracts;
using VideoPlayer.Entities;

namespace VideoPlayer.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagVideoContoller : ControllerBase
    {
        private readonly ITagVideoRepository _tagVideoRepository;

        public TagVideoContoller(ITagVideoRepository tagVideoRepository)
        {
            this._tagVideoRepository = tagVideoRepository;
        }

        //[HttpGet]
        //public async Task<List<TagVideo>> Get()
        //{
        //    return await this._tagVideoRepository.GetAsync();
        //} 
        
        [HttpGet]
        public List<TagVideo> Get()
        {
            return this._tagVideoRepository.Get();
        }
    }
}
