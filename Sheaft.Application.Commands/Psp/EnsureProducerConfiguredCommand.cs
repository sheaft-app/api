using Sheaft.Core;
using Newtonsoft.Json;
using System;
using Sheaft.Domain.Enums;

namespace Sheaft.Application.Commands
{
    public class EnsureProducerConfiguredCommand : Command<bool>
    {
        [JsonConstructor]
        public EnsureProducerConfiguredCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
    }
}
