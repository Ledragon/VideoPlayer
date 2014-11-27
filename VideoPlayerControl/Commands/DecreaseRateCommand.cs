using System;

namespace VideoPlayerControl.Commands
{
    public class DecreaseRateCommand : CommandBase
    {
        public DecreaseRateCommand(Action method) : base(method)
        {
        }
    }
}