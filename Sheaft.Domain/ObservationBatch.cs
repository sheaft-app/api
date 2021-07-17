using System;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain
{
    public class ObservationBatch : ITrackCreation
    { 
        protected ObservationBatch()
        {
        }
        
        public ObservationBatch(Batch batch)
        {
            BatchId = batch.Id;
            Batch = batch;
        }

        public DateTimeOffset CreatedOn { get; private set; }
        public Guid BatchId { get; private set; }
        public Guid ObservationId { get; private set; }
        public virtual Batch Batch { get; private set; }
    }
}