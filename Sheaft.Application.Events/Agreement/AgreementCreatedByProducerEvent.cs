namespace Sheaft.Application.Events
{
    public class AgreementCreatedByProducerEvent : AgreementEvent
    {
        public const string QUEUE_NAME = "agreementcreatedbyproducer";
        public const string MAILING_TEMPLATE_ID = "";

        public AgreementCreatedByProducerEvent(Interop.IRequestUser user) : base(user)
        {
        }
    }
}
