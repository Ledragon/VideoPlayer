using System;
using VideoPlayer.Infrastructure;

namespace VideoPlayerControl.Commands
{
    public class IncreaseRateCommand : CommandBase
    {
        public IncreaseRateCommand(Action method) : base(method)
        {
        }
    }
}