using Sheaft.Core;
using System;
using Newtonsoft.Json;

namespace Sheaft.Application.Events
{
    public class ProductImportProcessingEvent : Event
    {
        public const string QUEUE_NAME = "event-products-import-processing";

        [JsonConstructor]
        public ProductImportProcessingEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
    }
}
