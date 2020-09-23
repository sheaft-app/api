using System;
using Sheaft.Domain.Enums;
using Sheaft.Application.Models;
using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Commands
{
    public class EnsureConsumerLegalConfiguredCommand : Command<bool>
    {
        [JsonConstructor]
        public EnsureConsumerLegalConfiguredCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid UserId { get; set; }
    }
}
