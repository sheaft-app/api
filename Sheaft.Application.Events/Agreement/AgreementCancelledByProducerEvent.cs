using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Events
{
    public class AgreementCancelledByProducerEvent : AgreementEvent
    {
        public const string QUEUE_NAME = "event-agreements-cancelled-producer";
        public const string MAILING_TEMPLATE_ID = "";

        [JsonConstructor]
        public AgreementCancelledByProducerEvent(RequestUser requestUser) : base(requestUser)
        {
        }
    }
}
