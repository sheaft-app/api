using System;

namespace Sheaft.Domain
{
    public class ObservationBatch : SurveillanceBatch
    {
        protected ObservationBatch()
        {
        }

        public ObservationBatch(Batch batch)
            : base(batch)
        {
        }
        
        public Guid ObservationId { get; private set; }
    }
}