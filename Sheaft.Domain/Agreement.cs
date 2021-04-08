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
        private List<TimeSlotHour> _selectedHours;

        protected Agreement()
        {
        }

        public Agreement(Guid id, Store store, DeliveryMode delivery, User createdBy, IEnumerable<TimeSlotHour> deliveryHours)
        {
            Id = id;
            Delivery = delivery;
            Store = store;
            CreatedBy = createdBy;
            
            if(CreatedBy.Id == store.Id)
                Status = AgreementStatus.WaitingForProducerApproval;
            else
                Status = AgreementStatus.WaitingForStoreApproval;

            SetSelectedHours(deliveryHours);
            DomainEvents = new List<DomainEvent> {new AgreementCreatedEvent(Id)};
        }

        public Guid Id { get; private set; }
        public AgreementStatus Status { get; set; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset? UpdatedOn { get; private set; }
        public DateTimeOffset? RemovedOn { get; private set; }
        public string Reason { get; private set; }
        public virtual DeliveryMode Delivery { get; private set; }
        public virtual Store Store { get; private set; }
        public virtual User CreatedBy { get; private set; }
        public virtual Catalog Catalog { get; private set; }
        public virtual IReadOnlyCollection<TimeSlotHour> SelectedHours => _selectedHours?.AsReadOnly();
        public List<DomainEvent> DomainEvents { get; } = new List<DomainEvent>();

        public void SetSelectedHours(IEnumerable<TimeSlotHour> selectedHours)
        {
            _selectedHours = new List<TimeSlotHour>();
            foreach (var selectedHour in selectedHours)
            {
                var openingHours = Delivery.OpeningHours.FirstOrDefault(o => o.Day == selectedHour.Day && o.From == selectedHour.From && o.To == selectedHour.To);
                if (openingHours == null)
                    throw new ValidationException(MessageKind.Agreement_SelectedHour_NotFoundInDeliveryOpeningHours);

                _selectedHours.Add(new TimeSlotHour(openingHours.Day, openingHours.From, openingHours.To));
            }
        }

        public void AcceptAgreement()
        {
            if (Status != AgreementStatus.WaitingForProducerApproval && Status != AgreementStatus.WaitingForStoreApproval)
                throw new ValidationException(MessageKind.Agreement_CannotBeAccepted_NotInWaitingStatus);

            Status = AgreementStatus.Accepted;
            DomainEvents.Add(new AgreementAcceptedEvent(Id));
        }

        public void CancelAgreement(string reason)
        {
            if (Status == AgreementStatus.Cancelled)
                throw new ValidationException(MessageKind.Agreement_CannotBeCancelled_AlreadyCancelled);

            if (Status == AgreementStatus.Refused)
                throw new ValidationException(MessageKind.Agreement_CannotBeCancelled_AlreadyRefused);

            Status = AgreementStatus.Cancelled;
            Reason = reason;
            DomainEvents.Add(new AgreementCancelledEvent(Id));
        }

        public void RefuseAgreement(string reason)
        {
            if (Status != AgreementStatus.WaitingForProducerApproval && Status != AgreementStatus.WaitingForStoreApproval)
                throw new ValidationException(MessageKind.Agreement_CannotBeRefused_NotInWaitingStatus);

            Status = AgreementStatus.Refused;
            Reason = reason;
            DomainEvents.Add(new AgreementRefusedEvent(Id));
        }

        public void Reset()
        {
            if (CreatedBy.Kind == ProfileKind.Producer)
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

            Catalog = catalog;
        }
    }
}