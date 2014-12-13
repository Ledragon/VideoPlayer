using System;
using VideoPlayer.Infrastructure;

namespace VideoPlayerControl.Commands
{
    public class StopCommand:CommandBase
    {
        public StopCommand(Action method) : base(method)
        {
        }
    }
}