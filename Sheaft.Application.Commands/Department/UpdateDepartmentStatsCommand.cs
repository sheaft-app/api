using Newtonsoft.Json;
using Sheaft.Core;
using System;

namespace Sheaft.Application.Commands
{
    public class UpdateDepartmentStatsCommand : Command<bool>
    {
        public const string QUEUE_NAME = "command-zones-department-stats";

        [JsonConstructor]
        public UpdateDepartmentStatsCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
        public int Points { get; set; }
        public int Position { get; set; }
        public int Producers { get; set; }
        public int Stores { get; set; }
    }
}
