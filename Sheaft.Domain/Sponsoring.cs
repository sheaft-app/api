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
    public class Sponsoring: ITrackCreation, IHasDomainEvent
    {
        protected Sponsoring() { }

        public Sponsoring(User sponsor, User sponsored)
        {
            if(sponsor == null)
                throw SheaftException.Validation("Le sponsor est requis.");

            if(sponsored == null)
                throw SheaftException.Validation("Le sponsorisé est requis.");

            Sponsor = sponsor;
            SponsorId = sponsor.Id;
            Sponsored = sponsored;
            SponsoredId = sponsored.Id;
            
            DomainEvents = new List<DomainEvent>{new UserSponsoredEvent(sponsor.Id, sponsored.Id)};
        }

        public DateTimeOffset CreatedOn { get; private set; }
        public Guid SponsorId { get; private set; }
        public Guid SponsoredId { get; private set; }
        public virtual User Sponsor { get; private set; }
        public virtual User Sponsored { get; private set; }
        public List<DomainEvent> DomainEvents { get; } = new List<DomainEvent>();
    }
}