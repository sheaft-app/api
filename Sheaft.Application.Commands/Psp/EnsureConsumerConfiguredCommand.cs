using Sheaft.Core;
using Newtonsoft.Json;
using System;
using Sheaft.Domain.Enums;

namespace Sheaft.Application.Commands
{
    public class EnsureConsumerConfiguredCommand : Command<bool>
    {
        [JsonConstructor]
        public EnsureConsumerConfiguredCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
    }
}
