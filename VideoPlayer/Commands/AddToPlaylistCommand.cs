using System;

namespace VideoPlayer.Commands
{
    public class AddToPlaylistCommand:CommandBase
    {
        public AddToPlaylistCommand(Action method) : base(method)
        {
        }
    }
}