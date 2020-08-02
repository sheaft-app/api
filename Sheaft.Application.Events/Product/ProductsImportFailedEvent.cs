﻿using MediatR;
using System;

namespace Sheaft.Application.Events
{
    public class ProductImportFailedEvent : Event
    {
        public const string QUEUE_NAME = "eventproductimportfailed";
        public const string MAILING_TEMPLATE_ID = "d-b4a25a1730da4f358fc390ad4b179913";

        public ProductImportFailedEvent(Interop.IRequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
    }
}
