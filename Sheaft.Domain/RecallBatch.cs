using System;

namespace Sheaft.Domain
{
    public class RecallBatch : SurveillanceBatch
    {
        protected RecallBatch()
        {
        }

        public RecallBatch(Batch batch)
            : base(batch)
        {
        }
        
        public Guid RecallId { get; private set; }
    }
}