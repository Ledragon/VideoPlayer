using System;

namespace VideoPlayerControl.Commands
{
    public class SnapshotCommand : CommandBase
    {
        public SnapshotCommand(Action method) : base(method)
        {
        }
    }
}