﻿using Sheaft.Core;

namespace Sheaft.Application.Events
{
    public class AgreementRefusedByProducerEvent : AgreementEvent
    {
        public const string QUEUE_NAME = "event-agreements-refused-producer";
        public const string MAILING_TEMPLATE_ID = "";

        public AgreementRefusedByProducerEvent(RequestUser user) : base(user)
        {
        }
    }
}
