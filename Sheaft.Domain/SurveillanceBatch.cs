using System;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain
{
    public abstract class SurveillanceBatch : ITrackCreation
    {
        protected SurveillanceBatch()
        {
        }

        protected SurveillanceBatch(Batch batch)
        {
            BatchId = batch.Id;
            Batch = batch;
        }

        public DateTimeOffset CreatedOn { get; private set; }
        public Guid BatchId { get; private set; }
        public virtual Batch Batch { get; private set; }
    }
}