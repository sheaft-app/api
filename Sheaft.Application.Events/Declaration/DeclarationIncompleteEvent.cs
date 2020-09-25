using Newtonsoft.Json;
using Sheaft.Core;
using System;

namespace Sheaft.Application.Events
{
    public class DeclarationIncompleteEvent : Event
    {
        public const string QUEUE_NAME = "event-psp-declaration-incomplete";
        public const string MAILING_TEMPLATE_ID = "";

        [JsonConstructor]
        public DeclarationIncompleteEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid DeclarationId { get; set; }
    }
}
