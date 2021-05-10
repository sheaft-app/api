using System;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain
{
    public abstract class Picture: IIdEntity, ITrackCreation, ITrackUpdate
    {
        protected Picture()
        {
        }
        
        protected Picture(Guid id, string url)
        {
            Id = id;
            Url = url;
        }

        public Guid Id { get; private set; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset? UpdatedOn { get; private set; }
        public string Url { get; private set; }
    }
}