﻿using Sheaft.Core;
using System;
using Newtonsoft.Json;

namespace Sheaft.Application.Events
{
    public class ProductImportFailedEvent : Event
    {
        public const string QUEUE_NAME = "event-products-import-failed";
        public const string MAILING_TEMPLATE_ID = "d-b4a25a1730da4f358fc390ad4b179913";

        [JsonConstructor]
        public ProductImportFailedEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
    }
}
