using MediatR;
using System;

namespace Sheaft.Application.Events
{
    public abstract class AgreementEvent: Event
    {
        protected AgreementEvent(Interop.IRequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
    }
}
