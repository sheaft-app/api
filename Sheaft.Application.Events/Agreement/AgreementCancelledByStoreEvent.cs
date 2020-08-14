namespace Sheaft.Application.Events
{
    public class AgreementCancelledByStoreEvent : AgreementEvent
    {
        public const string QUEUE_NAME = "event-agreements-cancelled-store";
        public const string MAILING_TEMPLATE_ID = "";

        public AgreementCancelledByStoreEvent(Interop.IRequestUser user) : base(user)
        {
        }
    }
}
