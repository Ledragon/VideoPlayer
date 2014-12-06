using System;

namespace VideoPlayer.Commands
{
    public class FilterCommand:CommandBase
    {
        public FilterCommand(Action method) : base(method)
        {
        }
    }
}