using Sheaft.Exceptions;
using Sheaft.Interop;
using Sheaft.Interop.Enums;
using System;

namespace Sheaft.Domain.Models
{
    public class Sponsoring: ITrackCreation, ITrackRemove
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
        }

        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset? RemovedOn { get; private set; }
        public virtual User Sponsor { get; private set; }
        public virtual User Sponsored { get; private set; }
    }
}