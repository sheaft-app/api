using Sheaft.Core;
using System;

namespace Sheaft.Application.Commands
{
    public class UpdateRegionCommand : Command<bool>
    {
        public const string QUEUE_NAME = "command-regions-update";

        public UpdateRegionCommand(RequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
        public int Points { get; set; }
        public int Position { get; set; }
        public int Producers { get; set; }
        public int Stores { get; set; }
    }
}
