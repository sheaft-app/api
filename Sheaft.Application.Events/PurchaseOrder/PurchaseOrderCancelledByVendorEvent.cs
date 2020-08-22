﻿using Sheaft.Core;
using System;

namespace Sheaft.Application.Events
{
    public class PurchaseOrderCancelledByVendorEvent : Event
    {
        public const string QUEUE_NAME = "event-purchaseorders-cancelled-vendor";
        public const string MAILING_TEMPLATE_ID = "d-5a9e89b8b0564e8bba9ed6614e972454";

        public PurchaseOrderCancelledByVendorEvent(RequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
    }
}
