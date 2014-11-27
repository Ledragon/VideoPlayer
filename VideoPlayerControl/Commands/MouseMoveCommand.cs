using System;

namespace VideoPlayerControl.Commands
{
    public class MouseMoveCommand:CommandBase
    {
        public MouseMoveCommand(Action method) : base(method)
        {
        }
    }
}