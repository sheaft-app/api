using System;
using Sheaft.Domain.Enum;

namespace Sheaft.Web.Manage.Models
{
    public class PreAuthorizationViewModel
    {
        public Guid Id { get; set; }
        public string Identifier { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public DateTimeOffset? RemovedOn { get; set; }
        public DateTimeOffset? ExpirationDate { get; set; }
        public decimal Debited { get; set; }
        public decimal Remaining { get; set; }
        public PreAuthorizationStatus Status { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public string Reference { get; set; }
        public string ResultCode { get; set; }
        public string ResultMessage { get; set; }
        public bool SecureModeNeeded { get; set; }
        public string SecureModeRedirectUrl { get; set; }
        public string SecureModeReturnURL { get; set; }
        public Guid OrderId { get; set; }
        public Guid CardId { get; set; }
        public Guid? PreAuthorizedPayinId { get; set; }
        public virtual OrderViewModel Order { get; set; }
        public virtual CardViewModel Card { get; set; }
        public virtual PayinViewModel PreAuthorizedPayin { get; set; }
    }
}