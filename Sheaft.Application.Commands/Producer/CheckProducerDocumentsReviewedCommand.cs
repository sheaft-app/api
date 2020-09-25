using System;
using Sheaft.Core;
using Newtonsoft.Json;
using Sheaft.Domain.Enums;

namespace Sheaft.Application.Commands
{
    public class CheckProducerDocumentsReviewedCommand : Command<bool>
    {
        [JsonConstructor]
        public CheckProducerDocumentsReviewedCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid ProducerId { get; set; }
    }
}
