using Sheaft.Core;

namespace Sheaft.Application.Events
{
    public class AgreementRefusedByStoreEvent : AgreementEvent
    {
        public const string QUEUE_NAME = "event-agreements-refused-store";
        public const string MAILING_TEMPLATE_ID = "";

        public AgreementRefusedByStoreEvent(RequestUser user) : base(user)
        {
        }
    }
}
