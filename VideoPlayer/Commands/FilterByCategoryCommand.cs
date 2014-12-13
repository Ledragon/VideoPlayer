using System;
using VideoPlayer.Infrastructure;

namespace VideoPlayer.Commands
{
    public class FilterByCategoryCommand : CommandBase
    {
        public FilterByCategoryCommand(Action method) : base(method)
        {
        }
    }
}