namespace Sheaft.Application.Events
{
    public class AgreementAcceptedByStoreEvent : AgreementEvent
    {
        public const string QUEUE_NAME = "event-agreements-accepted-store";
        public const string MAILING_TEMPLATE_ID = "";

        public AgreementAcceptedByStoreEvent(Interop.IRequestUser user) : base(user)
        {
        }
    }
}
