using System;
using Sheaft.Domain.BaseClass;

namespace Sheaft.Domain
{
    public class RecallBatchNumber : SurveillanceBatchNumber
    {
        protected RecallBatchNumber()
        {
        }

        public RecallBatchNumber(BatchNumber batchNumber)
            : base(batchNumber)
        {
        }
        
        public Guid RecallId { get; private set; }
    }
}