using Sheaft.Core;
using System;
using Newtonsoft.Json;

namespace Sheaft.Application.Events
{
    public class PickingOrderExportSucceededEvent : Event
    {
        public const string QUEUE_NAME = "event-pickingorders-export-succeeded";
        public const string MAILING_TEMPLATE_ID = "d-45f0c494fa0f48a9a7024b667cea16a0";

        [JsonConstructor]
        public PickingOrderExportSucceededEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
    }
}
