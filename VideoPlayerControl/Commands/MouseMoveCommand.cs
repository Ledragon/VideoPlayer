using System;
using VideoPlayer.Infrastructure;

namespace VideoPlayerControl.Commands
{
    public class MouseMoveCommand:CommandBase
    {
        public MouseMoveCommand(Action method) : base(method)
        {
        }
    }
}