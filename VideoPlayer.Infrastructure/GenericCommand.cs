using System;

namespace VideoPlayer.Infrastructure
{
    public class GenericCommand : CommandBase
    {
        public GenericCommand(Action method) : base(method)
        {
        }
    }
}