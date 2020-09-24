using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Events
{
    public class AgreementCreatedEvent : AgreementEvent
    {
        public const string QUEUE_NAME = "event-agreements-created";
        public const string MAILING_TEMPLATE_ID = "";

        [JsonConstructor]
        public AgreementCreatedEvent(RequestUser requestUser) : base(requestUser)
        {
        }
    }
}
