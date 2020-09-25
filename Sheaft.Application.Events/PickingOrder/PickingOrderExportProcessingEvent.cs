using Sheaft.Core;
using System;
using Newtonsoft.Json;

namespace Sheaft.Application.Events
{
    public class PickingOrderExportProcessingEvent : Event
    {
        public const string QUEUE_NAME = "event-pickingorder-export-processing";

        [JsonConstructor]
        public PickingOrderExportProcessingEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid JobId { get; set; }
    }
}
