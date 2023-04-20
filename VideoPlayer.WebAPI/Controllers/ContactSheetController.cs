using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VideoPlayer.Database.Repository.Contracts;
using VideoPlayer.Entities;

namespace VideoPlayer.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactSheetController : ControllerBase
    {
        private readonly IContactSheetsRepository _contactSheetsRepository;

        public ContactSheetController(IContactSheetsRepository contactSheetsRepository)
        {
            this._contactSheetsRepository = contactSheetsRepository;
        }

        [HttpGet]
        public async Task<ContactSheet> GetForVideo([FromQuery] Int32 videoId)
        {
            return await this._contactSheetsRepository.GetForVideo(videoId);
        }
    }
}
