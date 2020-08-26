using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Events
{
    public class AgreementCreatedByProducerEvent : AgreementEvent
    {
        public const string QUEUE_NAME = "event-agreements-created-producer";
        public const string MAILING_TEMPLATE_ID = "";

        [JsonConstructor]
        public AgreementCreatedByProducerEvent(RequestUser requestUser) : base(requestUser)
        {
        }
    }
}
