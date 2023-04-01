using Microsoft.AspNetCore.Mvc;
using VideoPlayer.Database.Repository.Contracts;
using VideoPlayer.Services;

namespace VideoPlayer.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SettingsController : ControllerBase
    {
        private readonly IDirectoryRepository _directoryRepository;
        private readonly IRefreshService _refreshService;

        public SettingsController(IDirectoryRepository directoryRepository, IRefreshService refreshService)
        {
            this._directoryRepository = directoryRepository;
            this._refreshService = refreshService;
        }

        [HttpGet("directories")]
        public List<Entities.Directory> GetDirectories()
        {
            return this._directoryRepository.Get();
        }

        [HttpPost("updateVideos")]
        public void UpdateVideos()
        {
            this._directoryRepository.Get()
                .ForEach(directory =>
                {
                    this._refreshService.Load(directory);
                });
        }

        [HttpDelete("cleanVideos")]
        public void CleanVideos()
        {
            this._directoryRepository.Get()
                .ForEach(directory =>
                {
                    this._refreshService.Clean(directory);
                });
        }
    }
}
