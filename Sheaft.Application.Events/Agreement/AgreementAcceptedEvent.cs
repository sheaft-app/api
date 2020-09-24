using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Events
{
    public class AgreementAcceptedEvent : AgreementEvent
    {
        public const string QUEUE_NAME = "event-agreements-accepted";
        public const string MAILING_TEMPLATE_ID = "";

        [JsonConstructor]
        public AgreementAcceptedEvent(RequestUser requestUser) : base(requestUser)
        {
        }
    }
}
