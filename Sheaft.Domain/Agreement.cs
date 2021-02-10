using Sheaft.Domain.Enums;
using Sheaft.Domain.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using Sheaft.Domains.Exceptions;

namespace Sheaft.Domain.Models
{
    public class Agreement : IEntity
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
        public virtual IReadOnlyCollection<TimeSlotHour> SelectedHours => _selectedHours?.AsReadOnly();

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
        }

        public void CancelAgreement(string reason)
        {
            if (Status == AgreementStatus.Cancelled)
                throw new ValidationException(MessageKind.Agreement_CannotBeCancelled_AlreadyCancelled);

            if (Status == AgreementStatus.Refused)
                throw new ValidationException(MessageKind.Agreement_CannotBeCancelled_AlreadyRefused);

            Status = AgreementStatus.Cancelled;
            Reason = reason;
        }

        public void RefuseAgreement(string reason)
        {
            if (Status != AgreementStatus.WaitingForProducerApproval && Status != AgreementStatus.WaitingForStoreApproval)
                throw new ValidationException(MessageKind.Agreement_CannotBeRefused_NotInWaitingStatus);

            Status = AgreementStatus.Refused;
            Reason = reason;
        }

        public void Reset()
        {
            if (CreatedBy.Kind == ProfileKind.Producer)
                Status = AgreementStatus.WaitingForStoreApproval;
            else
                Status = AgreementStatus.WaitingForProducerApproval;

            Reason = null;
        }
    }
}