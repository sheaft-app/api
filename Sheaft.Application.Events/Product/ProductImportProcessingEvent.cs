using Sheaft.Core;
using System;

namespace Sheaft.Application.Events
{
    public class ProductImportProcessingEvent : Event
    {
        public const string QUEUE_NAME = "event-products-import-processing";

        public ProductImportProcessingEvent(RequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
    }
}
