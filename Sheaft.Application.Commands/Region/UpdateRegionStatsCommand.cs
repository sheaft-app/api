using Newtonsoft.Json;
using Sheaft.Core;
using System;

namespace Sheaft.Application.Commands
{
    public class UpdateRegionStatsCommand : Command<bool>
    {
        public const string QUEUE_NAME = "command-zones-region-stats";

        [JsonConstructor]
        public UpdateRegionStatsCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
        public int Points { get; set; }
        public int Position { get; set; }
        public int Producers { get; set; }
        public int Stores { get; set; }
    }
}
