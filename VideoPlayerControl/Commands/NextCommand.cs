using System;
using VideoPlayer.Infrastructure;

namespace VideoPlayerControl.Commands
{
    public class NextCommand : CommandBase
    {
        public NextCommand(Action method) : base(method)
        {
        }
    }
}