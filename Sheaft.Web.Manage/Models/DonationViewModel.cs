using System;
using Sheaft.Domain.Enum;

namespace Sheaft.Web.Manage.Models
{
    public class DonationViewModel
    {
        public Guid Id { get; set; }
        public string Identifier { get; set; }
        public TransactionKind Kind { get; set; }
        public TransactionStatus Status { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public decimal Fees { get; set; }
        public decimal Credited { get; set; }
        public decimal Debited { get; set; }
        public string Reference { get; set; }
        public string ResultCode { get; set; }
        public string ResultMessage { get; set; }
        public DateTimeOffset? ExecutedOn { get; set; }
        public UserProfileViewModel Author { get; set; }
        public UserProfileViewModel DebitedUser { get; set; }
        public UserProfileViewModel CreditedUser { get; set; }
        public ShortOrderViewModel Order { get; set; }
    }
}
