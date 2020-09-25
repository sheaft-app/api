using Newtonsoft.Json;
using Sheaft.Core;
using System;

namespace Sheaft.Application.Events
{
    public class DeclarationRefusedEvent : Event
    {
        public const string QUEUE_NAME = "event-declaration-refused";
        public const string MAILING_TEMPLATE_ID = "";

        [JsonConstructor]
        public DeclarationRefusedEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid DeclarationId { get; set; }
    }
}
