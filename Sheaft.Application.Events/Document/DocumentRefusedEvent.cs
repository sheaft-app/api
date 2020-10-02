using Newtonsoft.Json;
using Sheaft.Core;
using System;

namespace Sheaft.Application.Events
{
    public class DocumentRefusedEvent : Event
    {
        public const string MAILING_TEMPLATE_ID = "";

        [JsonConstructor]
        public DocumentRefusedEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid DocumentId { get; set; }
    }
}
