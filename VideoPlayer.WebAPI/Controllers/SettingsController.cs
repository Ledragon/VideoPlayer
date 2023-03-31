using Microsoft.AspNetCore.Mvc;
using VideoPlayer.Database.Repository.Contracts;

namespace VideoPlayer.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SettingsController : ControllerBase
    {
        private readonly IDirectoryRepository _directoryRepository;

        public SettingsController(IDirectoryRepository directoryRepository)
        {
            this._directoryRepository = directoryRepository;
        }

        [HttpGet("directories")]
        public List<Entities.Directory> GetDirectories()
        {
            return this._directoryRepository.Get();
        }
    }
}
