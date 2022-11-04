using System;

namespace VideoPlayer.Services
{
    public interface ISettingsService
    {
        String VlcFolder { get; }
    }
}