using System;

namespace VideoPlayer.Commands
{
    public class GenericCommand:CommandBase
    {
        public GenericCommand(Action method) : base(method)
        {
        }
    }
}