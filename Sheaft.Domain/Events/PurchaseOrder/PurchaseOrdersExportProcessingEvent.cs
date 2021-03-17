﻿using System;
using Newtonsoft.Json;
using Sheaft.Domain.Common;

namespace Sheaft.Domain.Events.Transactions
{
    public class PurchaseOrdersExportProcessingEvent : DomainEvent
    {
        [JsonConstructor]
        public PurchaseOrdersExportProcessingEvent(Guid jobId)
        {
            JobId = jobId;
        }

        public Guid JobId { get; }
    }
}
