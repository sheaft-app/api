using Newtonsoft.Json;
using Sheaft.Core;
using System;

namespace Sheaft.Application.Events
{
    public class DeclarationValidatedEvent : Event
    {
        public const string QUEUE_NAME = "event-psp-declaration-validated";
        public const string MAILING_TEMPLATE_ID = "";

        [JsonConstructor]
        public DeclarationValidatedEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid DeclarationId { get; set; }
    }
}
