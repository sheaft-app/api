using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Events
{
    public class AgreementCancelledEvent : AgreementEvent
    {
        public const string QUEUE_NAME = "event-agreements-cancelled";
        public const string MAILING_TEMPLATE_ID = "";

        [JsonConstructor]
        public AgreementCancelledEvent(RequestUser requestUser) : base(requestUser)
        {
        }
    }
}
