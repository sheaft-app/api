using Newtonsoft.Json;
using Sheaft.Core;
using System;

namespace Sheaft.Application.Commands
{
    public class UpdateRegionCommand : Command<bool>
    {
        public const string QUEUE_NAME = "command-regions-update";

        [JsonConstructor]
        public UpdateRegionCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
        public int Points { get; set; }
        public int Position { get; set; }
        public int Producers { get; set; }
        public int Stores { get; set; }
    }
}
