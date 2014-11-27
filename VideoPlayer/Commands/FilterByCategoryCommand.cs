using System;

namespace VideoPlayer.Commands
{
    public class FilterByCategoryCommand : CommandBase
    {
        public FilterByCategoryCommand(Action method) : base(method)
        {
        }
    }
}