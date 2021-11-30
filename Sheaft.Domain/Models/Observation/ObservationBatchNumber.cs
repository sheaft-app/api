using System;
using Sheaft.Domain.BaseClass;

namespace Sheaft.Domain
{
    public class ObservationBatchNumber : SurveillanceBatchNumber
    {
        protected ObservationBatchNumber()
        {
        }

        public ObservationBatchNumber(BatchNumber batchNumber)
            : base(batchNumber)
        {
        }
    }
}