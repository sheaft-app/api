﻿using Newtonsoft.Json;
using Sheaft.Core;
using System;

namespace Sheaft.Application.Events
{
    public class TransferRefundSucceededEvent : Event
    {
        public const string QUEUE_NAME = "event-transfer-refund-succeeded";
        public const string MAILING_TEMPLATE_ID = "";

        [JsonConstructor]
        public TransferRefundSucceededEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid RefundId { get; set; }
    }
}