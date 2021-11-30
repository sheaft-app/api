using System;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain
{
    public class PickingBatchNumber
    {
        protected PickingBatchNumber()
        {
        }

        public PickingBatchNumber(Guid batchNumberId)
        {
            BatchNumberId = batchNumberId;
        }

        public Guid BatchNumberId { get; private set; }
    }
}