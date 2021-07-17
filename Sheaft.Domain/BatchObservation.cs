using System;
using System.Collections.Generic;
using Sheaft.Core.Exceptions;
using Sheaft.Domain.Common;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Events.BatchObservation;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain
{
    public class BatchObservation : IIdEntity, ITrackCreation, ITrackUpdate, IHasDomainEvent
    {
        protected BatchObservation()
        {
        }
        
        public BatchObservation(Guid id, string comment, User user, Guid? batchId = null)
        {
            Id = id;
            Comment = comment;
            UserId = user.Id;
            User = user;

            if (batchId.HasValue)
                BatchId = batchId.Value;
        }

        public Guid Id { get; private set; }
        public string Comment { get; private set; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset? UpdatedOn { get; private set; }
        public Guid UserId { get; private set; }
        public Guid BatchId { get; private set; }
        public Guid? ReplyToId { get; private set; }
        public bool VisibleToAll { get; private set; }
        public virtual User User { get; private set; }
        public virtual ICollection<BatchObservation> Replies { get; private set; }
        public List<DomainEvent> DomainEvents { get; } = new List<DomainEvent>();

        public void SetComment(string comment)
        {
            Comment = comment;
        }
        
        public void AddReply(string comment, User user)
        {
            if(ReplyToId.HasValue)
                throw SheaftException.Validation("Une réponse ne peut être ajoutée qu'à une observation.");
            
            if (Replies == null)
                Replies = new List<BatchObservation>();

            var reply = new BatchObservation(Guid.NewGuid(), comment, user, BatchId);
            Replies.Add(reply);

            DomainEvents.Add(new BatchObservationRepliedEvent(Id, reply.Id));
        }

        public void SetVisibility(bool visibleToAll)
        {
            if (ReplyToId.HasValue && visibleToAll)
                throw SheaftException.Validation("Une réponse à une observation ne peut pas être publique.");
            
            if(User.Kind != ProfileKind.Producer && visibleToAll)
                throw SheaftException.Validation("Seul les observations d'un producteur peuvent être publique.");
            
            VisibleToAll = visibleToAll;
        }
    }
}