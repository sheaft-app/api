using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Events
{
    public class AgreementAcceptedByProducerEvent : AgreementEvent
    {
        public const string QUEUE_NAME = "event-agreements-accepted-producer";
        public const string MAILING_TEMPLATE_ID = "";

        [JsonConstructor]
        public AgreementAcceptedByProducerEvent(RequestUser requestUser) : base(requestUser)
        {
        }
    }
}
