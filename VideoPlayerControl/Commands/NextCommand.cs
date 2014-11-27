using System;

namespace VideoPlayerControl.Commands
{
    public class NextCommand : CommandBase
    {
        public NextCommand(Action method) : base(method)
        {
        }
    }
}