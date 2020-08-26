using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Events
{
    public class AgreementCancelledByStoreEvent : AgreementEvent
    {
        public const string QUEUE_NAME = "event-agreements-cancelled-store";
        public const string MAILING_TEMPLATE_ID = "";

        [JsonConstructor]
        public AgreementCancelledByStoreEvent(RequestUser requestUser) : base(requestUser)
        {
        }
    }
}
