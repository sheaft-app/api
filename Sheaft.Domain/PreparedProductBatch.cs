using System;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain
{
    public class PreparedProductBatch : ITrackCreation
    {
        protected PreparedProductBatch()
        {
        }

        public PreparedProductBatch(Batch batch)
        {
            BatchId = batch.Id;
            Batch = batch;
        }

        public DateTimeOffset CreatedOn { get; private set; }

        public Guid PreparedProductId { get; private set; }
        public Guid BatchId { get; private set; }
        public virtual Batch Batch { get; private set; }
    }
}