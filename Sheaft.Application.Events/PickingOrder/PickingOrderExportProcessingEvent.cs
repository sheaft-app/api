using Sheaft.Core;
using System;
using Newtonsoft.Json;

namespace Sheaft.Application.Events
{
    public class PickingOrderExportProcessingEvent : Event
    {
        public const string QUEUE_NAME = "event-pickingorders-export-processing";

        [JsonConstructor]
        public PickingOrderExportProcessingEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
    }
}
