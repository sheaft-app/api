using System;
using System.Collections.Generic;
using System.Linq;
using Sheaft.Core.Enums;
using Sheaft.Core.Exceptions;
using Sheaft.Domain.Common;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Events.Agreement;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain
{
    public class Agreement : IEntity, IHasDomainEvent
    {
        protected Agreement()
        {
        }

        public Agreement(Guid id, Store store, Producer producer, ProfileKind createdByKind, DeliveryMode delivery = null)
        {
            Id = id;
            StoreId = store.Id;
            Store = store;
            CreatedByKind = createdByKind;
            DeliveryId = delivery?.Id;
            Delivery = delivery;
            ProducerId = producer.Id;
            Producer = producer;
            
            if(CreatedByKind == ProfileKind.Store)
                Status = AgreementStatus.WaitingForProducerApproval;
            else
            {
                Status = AgreementStatus.WaitingForStoreApproval;
                if (delivery == null)
                    throw SheaftException.Validation();
            }

            DomainEvents = new List<DomainEvent> {new AgreementCreatedEvent(Id, createdByKind)};
        }

        public Guid Id { get; private set; }
        public AgreementStatus Status { get; set; }
        public ProfileKind CreatedByKind { get; private set; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset? UpdatedOn { get; private set; }
        public DateTimeOffset? RemovedOn { get; private set; }
        public string Reason { get; private set; }
        public Guid? DeliveryId { get; private set; }
        public Guid StoreId { get; private set; }
        public Guid ProducerId { get; private set; }
        public Guid? CatalogId { get; private set; }
        public virtual DeliveryMode Delivery { get; private set; }
        public virtual Store Store { get; private set; }
        public virtual Producer Producer { get; private set; }
        public virtual Catalog Catalog { get; private set; }
        public List<DomainEvent> DomainEvents { get; } = new List<DomainEvent>();
        public byte[] RowVersion { get; private set; }

        public void AcceptAgreement(DeliveryMode delivery, ProfileKind acceptedByKind)
        {
            if (Status != AgreementStatus.WaitingForProducerApproval && Status != AgreementStatus.WaitingForStoreApproval)
                throw SheaftException.Validation(MessageKind.Agreement_CannotBeAccepted_NotInWaitingStatus);

            if(Status == AgreementStatus.WaitingForProducerApproval && acceptedByKind != ProfileKind.Producer)
                throw SheaftException.Validation();
            
            if(Status == AgreementStatus.WaitingForStoreApproval && acceptedByKind != ProfileKind.Store)
                throw SheaftException.Validation();
            
            if(DeliveryId.HasValue && delivery != null && DeliveryId != delivery.Id)
                throw SheaftException.Validation();

            if (delivery != null)
            {
                DeliveryId = delivery.Id;
                Delivery = delivery;
            }

            if(!DeliveryId.HasValue)
                throw SheaftException.Validation();

            Status = AgreementStatus.Accepted;
            DomainEvents.Add(new AgreementAcceptedEvent(Id, acceptedByKind));
        }

        public void CancelAgreement(string reason, ProfileKind cancelledByKind)
        {
            if (Status == AgreementStatus.Cancelled)
                throw new ValidationException(MessageKind.Agreement_CannotBeCancelled_AlreadyCancelled);

            if (Status == AgreementStatus.Refused)
                throw new ValidationException(MessageKind.Agreement_CannotBeCancelled_AlreadyRefused);

            Status = AgreementStatus.Cancelled;
            Reason = reason;
            DomainEvents.Add(new AgreementCancelledEvent(Id, cancelledByKind));
        }

        public void RefuseAgreement(string reason, ProfileKind refusedByKind)
        {
            if (Status != AgreementStatus.WaitingForProducerApproval && Status != AgreementStatus.WaitingForStoreApproval)
                throw new ValidationException(MessageKind.Agreement_CannotBeRefused_NotInWaitingStatus);

            if(Status == AgreementStatus.WaitingForProducerApproval && refusedByKind != ProfileKind.Producer)
                throw SheaftException.Validation();
            
            if(Status == AgreementStatus.WaitingForStoreApproval && refusedByKind != ProfileKind.Store)
                throw SheaftException.Validation();
            
            Status = AgreementStatus.Refused;
            Reason = reason;
            DomainEvents.Add(new AgreementRefusedEvent(Id, refusedByKind));
        }

        public void Reset()
        {
            if (CreatedByKind == ProfileKind.Producer)
                Status = AgreementStatus.WaitingForStoreApproval;
            else
                Status = AgreementStatus.WaitingForProducerApproval;

            Reason = null;
        }

        public void AssignCatalog(Catalog catalog)
        {
            if (catalog.Kind == CatalogKind.Consumers)
                throw SheaftException.Validation();
            
            if(!catalog.Available)
                throw SheaftException.Validation();

            CatalogId = catalog.Id;
            Catalog = catalog;
        }
    }
}