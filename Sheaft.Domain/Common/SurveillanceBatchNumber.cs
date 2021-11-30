using System;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain.BaseClass
{
    public abstract class SurveillanceBatchNumber : ITrackUpdate
    {
        protected SurveillanceBatchNumber()
        {
        }

        protected SurveillanceBatchNumber(BatchNumber batchNumber)
        {
            BatchNumberId = batchNumber.Id;
        }

        public Guid Id { get; } = Guid.NewGuid();
        public DateTimeOffset UpdatedOn { get; private set; }
        public Guid BatchNumberId { get; private set; }
    }
}