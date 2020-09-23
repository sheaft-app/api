using System;
using Sheaft.Core;
using Newtonsoft.Json;
using Sheaft.Domain.Enums;

namespace Sheaft.Application.Commands
{
    public class EnsureProducerDocumentsReviewedCommand : Command<bool>
    {
        [JsonConstructor]
        public EnsureProducerDocumentsReviewedCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid UserId { get; set; }
    }
}
