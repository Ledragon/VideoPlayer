using System;
using VideoPlayer.Infrastructure;

namespace VideoPlayer.Commands
{
    public class FilterCommand:CommandBase
    {
        public FilterCommand(Action method) : base(method)
        {
        }
    }
}