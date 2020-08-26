using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Events
{
    public class AgreementCreatedByStoreEvent : AgreementEvent
    {
        public const string QUEUE_NAME = "event-agreements-created-store";
        public const string MAILING_TEMPLATE_ID = "";

        [JsonConstructor]
        public AgreementCreatedByStoreEvent(RequestUser requestUser) : base(requestUser)
        {
        }
    }
}
