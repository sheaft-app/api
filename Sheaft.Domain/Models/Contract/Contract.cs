using System;
using System.Collections.Generic;
using System.Linq;
using Sheaft.Domain.Common;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Events;
using Sheaft.Domain.Events.Contract;
using Sheaft.Domain.Exceptions;
using Sheaft.Domain.Interop;
using Sheaft.Domain.Security;

namespace Sheaft.Domain
{
    public class Contract : IEntity, IHasDomainEvent
    {
        protected Contract()
        {
        }

        public Contract(Company client, Company supplier, User createdBy, Distribution distribution = null)
        {
            Id = Guid.NewGuid();
            ClientId = client.Id;
            Client = client;
            DistributionId = distribution?.Id;
            Distribution = distribution;
            SupplierId = supplier.Id;
            Supplier = supplier;
            CreatedById = createdBy.Id;
            CreatedBy = createdBy;

            if (CreatedBy.Roles.Any(r => r.Role.Name == RoleDefinition.Store))
            {
                Status = ContractStatus.WaitingForSupplierApproval;
            }
            else
            {
                Status = ContractStatus.WaitingForStoreApproval;
                if (distribution == null)
                    throw new ValidationException("Le mode de livraison est requis pour créer un partenariat avec un magasin.");
            }

            DomainEvents = new List<DomainEvent> {new AgreementCreatedEvent(Id)};
        }

        public Guid Id { get; }
        public ContractStatus Status { get; private set; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset UpdatedOn { get; private set; }
        public bool Removed { get; private set; }
        
        public void Restore()
        {
            Removed = false;
        }

        public string Reason { get; private set; }
        public Guid CreatedById { get; private set; }
        public Guid ClientId { get; private set; }
        public Guid SupplierId { get; private set; }
        public Guid? DistributionId { get; private set; }
        public Guid? CatalogId { get; private set; }
        public User CreatedBy { get; private set; }
        public Company Client { get; private set; }
        public Company Supplier { get; private set; }
        public Catalog Catalog { get; private set; }
        public Distribution Distribution { get; private set; }
        public List<DomainEvent> DomainEvents { get; } = new List<DomainEvent>();
        public byte[] RowVersion { get; private set; }

        public void AcceptAgreement(Distribution delivery, User acceptedBy)
        {
            if (Status != ContractStatus.WaitingForSupplierApproval &&
                Status != ContractStatus.WaitingForStoreApproval)
                throw new ValidationException("Le partenariat ne peut pas être accepté, il n'est en attente d'acceptation.");

            if(Status == ContractStatus.WaitingForSupplierApproval && acceptedBy.Roles.All(r => r.Role.Name != RoleDefinition.Supplier))
                throw new ValidationException("Le partenariat doit être accepté par le producteur.");
            
            if(Status == ContractStatus.WaitingForStoreApproval && acceptedBy.Roles.All(r => r.Role.Name != RoleDefinition.Store))
                throw new ValidationException("Le partenariat doit être accepté par le magasin.");

            if (delivery != null)
                ChangeDelivery(delivery);

            if (!DistributionId.HasValue)
                throw new ValidationException("Le partenariat doit avoir un mode de livraison rattaché.");
            
            Status = ContractStatus.Accepted;
            DomainEvents.Add(new ContractAcceptedEvent(Id));
        }

        public void CancelAgreement(string reason)
        {
            if (Status == ContractStatus.Cancelled)
                throw new ValidationException("Le partenariat est déjà annulé.");

            if (Status == ContractStatus.Refused)
                throw new ValidationException("Le partenariat est déjà refusé.");

            Status = ContractStatus.Cancelled;
            Reason = reason;
            
            DomainEvents.Add(new ContractCancelledEvent(Id));
        }

        public void RefuseAgreement(string reason, User refusedBy)
        {
            if (Status != ContractStatus.WaitingForSupplierApproval &&
                Status != ContractStatus.WaitingForStoreApproval)
                throw new ValidationException("Le partenariat n'est pas en attente d'acceptation.");

            if (Status == ContractStatus.WaitingForSupplierApproval && refusedBy.Roles.All(r => r.Role.Name != RoleDefinition.Supplier))
                throw new ValidationException("Le partenariat ne peut être refusé que par le producteur.");
            
            if(Status == ContractStatus.WaitingForStoreApproval && refusedBy.Roles.All(r => r.Role.Name != RoleDefinition.Store))
                throw new ValidationException("Le partenariat ne peut être refusé que par le magasin.");

            Status = ContractStatus.Refused;
            Reason = reason;
            DomainEvents.Add(new AgreementRefusedEvent(Id));
        }

        public void Reset()
        {
            if (CreatedBy.Roles.All(r => r.Role.Name == RoleDefinition.Supplier))
                Status = ContractStatus.WaitingForStoreApproval;
            else
                Status = ContractStatus.WaitingForSupplierApproval;

            Reason = null;
        }

        public void ChangeCatalog(Catalog catalog)
        {
            if (catalog.Kind == CatalogKind.Consumers)
                throw new ValidationException("Impossible d'assigner un catalogue pour consommateur à un partenariat.");
            
            CatalogId = catalog.Id;
            Catalog = catalog;
        }

        public void ChangeDelivery(Distribution distribution)
        {
            DistributionId = distribution.Id;
            Distribution = distribution;
        }
    }
}