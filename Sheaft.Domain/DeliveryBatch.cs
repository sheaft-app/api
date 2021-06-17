using System;
using System.Collections.Generic;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain
{
    public class DeliveryBatch : IIdEntity, ITrackCreation, ITrackUpdate, ITrackRemove
    {
        protected DeliveryBatch()
        {
        }

        public DeliveryBatch(Guid id, DateTimeOffset scheduledOn)
        {
            Id = id;
            ScheduledOn = scheduledOn;
            Status = BatchStatus.Waiting;
        }

        public Guid Id { get; private set; }

        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset? UpdatedOn { get; private set; }
        public DateTimeOffset? RemovedOn { get; private set; }
        public BatchStatus Status { get; private set; }
        public int ProductsCount { get; private set; }
        public int DeliveriesCount { get; private set; }
        public DateTimeOffset ScheduledOn { get; private set; }
        public DateTimeOffset? StartedOn { get; private set; }
        public DateTimeOffset? CompletedOn { get; private set; }
        public string AssignedTo { get; private set; }
        
        public virtual ICollection<PurchaseOrderDelivery> Deliveries { get; private set; }
    }
}