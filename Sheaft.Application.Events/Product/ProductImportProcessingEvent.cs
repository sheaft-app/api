using MediatR;
using System;

namespace Sheaft.Application.Events
{
    public class ProductImportProcessingEvent : Event
    {
        public const string QUEUE_NAME = "event-products-import-processing";

        public ProductImportProcessingEvent(Interop.IRequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
    }
}
