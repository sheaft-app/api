using Sheaft.Core;
using System;

namespace Sheaft.Application.Events
{
    public abstract class AgreementEvent: Event
    {
        protected AgreementEvent(RequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
    }
}
