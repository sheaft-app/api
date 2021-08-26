using System;
using System.Collections.Generic;
using System.Linq;
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
            DeliveryModeId = delivery?.Id;
            DeliveryMode = delivery;
            ProducerId = producer.Id;
            Producer = producer;

            if (CreatedByKind == ProfileKind.Store)
            {
                Status = AgreementStatus.WaitingForProducerApproval;
            }
            else
            {
                Status = AgreementStatus.WaitingForStoreApproval;
                if (delivery == null)
                    throw SheaftException.Validation("Le mode de livraison est requis pour créer un partenariat avec un magasin.");
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
        public int? Position { get; private set; }
        public Guid StoreId { get; private set; }
        public Guid ProducerId { get; private set; }
        public Guid? DeliveryModeId { get; private set; }
        public Guid? CatalogId { get; private set; }
        public virtual Store Store { get; private set; }
        public virtual Producer Producer { get; private set; }
        public virtual DeliveryMode DeliveryMode { get; private set; }
        public virtual Catalog Catalog { get; private set; }
        public List<DomainEvent> DomainEvents { get; } = new List<DomainEvent>();
        public byte[] RowVersion { get; private set; }

        public void AcceptAgreement(DeliveryMode delivery, ProfileKind acceptedByKind)
        {
            if (Status != AgreementStatus.WaitingForProducerApproval &&
                Status != AgreementStatus.WaitingForStoreApproval)
                throw SheaftException.Validation("Le partenariat ne peut pas être accepté, il n'est en attente d'acceptation.");

            if(Status == AgreementStatus.WaitingForProducerApproval && acceptedByKind != ProfileKind.Producer)
                throw SheaftException.Validation("Le partenariat doit être accepté par le producteur.");
            
            if(Status == AgreementStatus.WaitingForStoreApproval && acceptedByKind != ProfileKind.Store)
                throw SheaftException.Validation("Le partenariat doit être accepté par le magasin.");

            if (delivery != null)
                ChangeDelivery(delivery);

            if (!DeliveryModeId.HasValue)
                throw SheaftException.Validation("Le partenariat doit avoir un mode de livraison rattaché.");

            Store.IncreaseProducersCount();
            
            Status = AgreementStatus.Accepted;
            DomainEvents.Add(new AgreementAcceptedEvent(Id, acceptedByKind));
        }

        public void CancelAgreement(string reason, ProfileKind cancelledByKind)
        {
            if (Status == AgreementStatus.Cancelled)
                throw SheaftException.Validation("Le partenariat est déjà annulé.");

            if (Status == AgreementStatus.Refused)
                throw SheaftException.Validation("Le partenariat est déjà refusé.");

            Status = AgreementStatus.Cancelled;
            Position = null;
            Reason = reason;
            
            Store.DecreaseProducersCount();
            DomainEvents.Add(new AgreementCancelledEvent(Id, cancelledByKind));
        }

        public void RefuseAgreement(string reason, ProfileKind refusedByKind)
        {
            if (Status != AgreementStatus.WaitingForProducerApproval &&
                Status != AgreementStatus.WaitingForStoreApproval)
                throw SheaftException.Validation("Le partenariat n'est pas en attente d'acceptation.");

            if (Status == AgreementStatus.WaitingForProducerApproval && refusedByKind != ProfileKind.Producer)
                throw SheaftException.Validation("Le partenariat ne peut être refusé que par le producteur.");
            
            if(Status == AgreementStatus.WaitingForStoreApproval && refusedByKind != ProfileKind.Store)
                throw SheaftException.Validation("Le partenariat ne peut être refusé que par le magasin.");

            Status = AgreementStatus.Refused;
            Position = null;
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
            Position = null;
        }

        public void ChangeCatalog(Catalog catalog)
        {
            if (catalog.Kind == CatalogKind.Consumers)
                throw SheaftException.Validation("Impossible d'assigner un catalogue particulier à un partenariat.");
            
            CatalogId = catalog.Id;
            Catalog = catalog;
        }

        public void ChangeDelivery(DeliveryMode deliveryMode)
        {
            DeliveryModeId = deliveryMode.Id;
            DeliveryMode = deliveryMode;
            Position = deliveryMode.Agreements.Count(a => a.Position.HasValue);
        }

        public void SetPosition(int position)
        {
            Position = position;
        }
    }
}