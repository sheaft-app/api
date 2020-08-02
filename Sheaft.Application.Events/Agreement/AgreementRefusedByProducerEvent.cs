namespace Sheaft.Application.Events
{
    public class AgreementRefusedByProducerEvent : AgreementEvent
    {
        public const string QUEUE_NAME = "agreementrefusedbyproducer";
        public const string MAILING_TEMPLATE_ID = "";

        public AgreementRefusedByProducerEvent(Interop.IRequestUser user) : base(user)
        {
        }
    }
}
