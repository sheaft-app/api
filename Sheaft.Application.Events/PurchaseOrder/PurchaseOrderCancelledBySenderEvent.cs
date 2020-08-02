﻿using MediatR;
using System;

namespace Sheaft.Application.Events
{
    public class PurchaseOrderCancelledBySenderEvent : Event
    {
        public const string QUEUE_NAME = "eventpurchaseordercancelledbysender";
        public const string MAILING_TEMPLATE_ID = "d-6cd1a1b5d8d444a7a987ee8980d84ef3";

        public PurchaseOrderCancelledBySenderEvent(Interop.IRequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
    }
}
