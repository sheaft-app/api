using System;
using System.Collections.Generic;
using Sheaft.Core.Enums;
using Sheaft.Core.Exceptions;
using Sheaft.Domain.Common;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Events.User;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain
{
    public class Sponsoring: ITrackCreation, ITrackRemove, IHasDomainEvent
    {
        protected Sponsoring() { }

        public Sponsoring(User sponsor, User sponsored)
        {
            if(sponsor == null)
                throw new ValidationException(MessageKind.Sponsoring_Sponsor_Required);

            if(sponsored == null)
                throw new ValidationException(MessageKind.Sponsoring_Sponsored_Required);

            Sponsor = sponsor;
            Sponsored = sponsored;
            DomainEvents = new List<DomainEvent>{new UserSponsoredEvent(sponsor.Id, sponsored.Id)};
        }

        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset? RemovedOn { get; private set; }
        public virtual User Sponsor { get; private set; }
        public virtual User Sponsored { get; private set; }
        public List<DomainEvent> DomainEvents { get; } = new List<DomainEvent>();
    }
}