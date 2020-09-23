using System;
using Sheaft.Core;
using Newtonsoft.Json;
using Sheaft.Domain.Enums;

namespace Sheaft.Application.Commands
{
    public class EnsureProducerDocumentsValidatedCommand : Command<bool>
    {
        [JsonConstructor]
        public EnsureProducerDocumentsValidatedCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid UserId { get; set; }
    }
}
