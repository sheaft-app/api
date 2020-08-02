using Sheaft.Exceptions;
using Sheaft.Interop;
using Sheaft.Interop.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sheaft.Domain.Models
{
    public class Agreement : IEntity
    {
        private List<TimeSlotHour> _selectedHours;

        protected Agreement()
        {
        }

        public Agreement(Guid id, Company store, DeliveryMode delivery, User createdBy, IEnumerable<TimeSlotHour> deliveryHours)
        {
            Id = id;
            Delivery = delivery;
            Store = store;

            if(createdBy.Company.Id == store.Id)
                Status = AgreementStatusKind.WaitingForProducerApproval;
            else
                Status = AgreementStatusKind.WaitingForStoreApproval;

            SetSelectedHours(deliveryHours);
        }

        public Guid Id { get; private set; }
        public AgreementStatusKind Status { get; set; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset? UpdatedOn { get; private set; }
        public DateTimeOffset? RemovedOn { get; private set; }
        public string Reason { get; private set; }
        public virtual DeliveryMode Delivery { get; private set; }
        public virtual Company Store { get; private set; }
        public virtual IReadOnlyCollection<TimeSlotHour> SelectedHours { get { return _selectedHours.AsReadOnly(); } private set { } }

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
            if (Status != AgreementStatusKind.WaitingForProducerApproval && Status != AgreementStatusKind.WaitingForStoreApproval)
                throw new ValidationException(MessageKind.Agreement_CannotBeAccepted_NotInWaitingStatus);

            Status = AgreementStatusKind.Accepted;
        }

        public void CancelAgreement(string reason)
        {
            if (Status == AgreementStatusKind.Cancelled)
                throw new ValidationException(MessageKind.Agreement_CannotBeCancelled_AlreadyCancelled);

            if (Status == AgreementStatusKind.Refused)
                throw new ValidationException(MessageKind.Agreement_CannotBeCancelled_AlreadyRefused);

            Status = AgreementStatusKind.Cancelled;
            Reason = reason;
        }

        public void RefuseAgreement(string reason)
        {
            if (Status != AgreementStatusKind.WaitingForProducerApproval && Status != AgreementStatusKind.WaitingForStoreApproval)
                throw new ValidationException(MessageKind.Agreement_CannotBeRefused_NotInWaitingStatus);

            Status = AgreementStatusKind.Refused;
            Reason = reason;
        }
    }
}