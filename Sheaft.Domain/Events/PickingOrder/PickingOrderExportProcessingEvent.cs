using System;
using Newtonsoft.Json;
using Sheaft.Core;
using Sheaft.Domain.Models.Common;

namespace Sheaft.Application.Events
{
    public class PickingOrderExportProcessingEvent : DomainEvent
    {
        [JsonConstructor]
        public PickingOrderExportProcessingEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid JobId { get; set; }
    }
}
