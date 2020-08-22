using Sheaft.Core;

namespace Sheaft.Application.Events
{
    public class AgreementCancelledByProducerEvent : AgreementEvent
    {
        public const string QUEUE_NAME = "event-agreements-cancelled-producer";
        public const string MAILING_TEMPLATE_ID = "";

        public AgreementCancelledByProducerEvent(RequestUser user) : base(user)
        {
        }
    }
}
