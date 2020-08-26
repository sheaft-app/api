using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Events
{
    public class AgreementRefusedByProducerEvent : AgreementEvent
    {
        public const string QUEUE_NAME = "event-agreements-refused-producer";
        public const string MAILING_TEMPLATE_ID = "";

        [JsonConstructor]
        public AgreementRefusedByProducerEvent(RequestUser requestUser) : base(requestUser)
        {
        }
    }
}
