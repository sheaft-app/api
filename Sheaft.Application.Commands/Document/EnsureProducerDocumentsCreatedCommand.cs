using System;
using Sheaft.Core;
using Newtonsoft.Json;
using Sheaft.Domain.Enums;

namespace Sheaft.Application.Commands
{
    public class EnsureProducerDocumentsCreatedCommand : Command<bool>
    {
        [JsonConstructor]
        public EnsureProducerDocumentsCreatedCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid UserId { get; set; }
    }
}
