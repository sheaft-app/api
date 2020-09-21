using Sheaft.Domain.Enums;
using System;
using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Commands
{
    public class UpdateRegionCommand : Command<bool>
    {
        [JsonConstructor]
        public UpdateRegionCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public int? RequiredProducers { get; set; }
    }
}
