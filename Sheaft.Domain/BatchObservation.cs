using System;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain
{
    public class BatchObservation : IIdEntity, ITrackCreation, ITrackUpdate
    {
        protected BatchObservation()
        {
        }

        public BatchObservation(Guid id, string comment, User user)
        {
            Id = id;
            Comment = comment;
            UserId = user.Id;
            User = user;
        }

        public Guid Id { get; private set; }
        public string Comment { get; private set; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset? UpdatedOn { get; private set; }
        public Guid UserId { get; private set; }
        public Guid BatchId { get; private set; }
        public virtual User User { get; private set; }
        
        public void SetComment(string comment)
        {
            Comment = comment;
        }
    }
}