using Sheaft.Core;

namespace Sheaft.Application.Events
{
    public class AgreementCreatedByProducerEvent : AgreementEvent
    {
        public const string QUEUE_NAME = "event-agreements-created-producer";
        public const string MAILING_TEMPLATE_ID = "";

        public AgreementCreatedByProducerEvent(RequestUser user) : base(user)
        {
        }
    }
}
