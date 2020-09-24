using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Events
{
    public class AgreementRefusedEvent : AgreementEvent
    {
        public const string QUEUE_NAME = "event-agreements-refused";
        public const string MAILING_TEMPLATE_ID = "";

        [JsonConstructor]
        public AgreementRefusedEvent(RequestUser requestUser) : base(requestUser)
        {
        }
    }
}
