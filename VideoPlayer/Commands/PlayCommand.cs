using System;
using VideoPlayer.Infrastructure;

namespace VideoPlayer.Commands
{
    public class PlayCommand : CommandBase
    {
        public PlayCommand(Action method) : base(method)
        {
        }
    }
}