using Sheaft.Interop.Enums;
using System;

namespace Sheaft.Models.Dto
{

    public class TransactionDto : BaseTransactionDto
    {
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public decimal Fees { get; set; }
        public decimal Debited { get; set; }
        public decimal Credited { get; set; }
        public DateTimeOffset? ExecutedOn { get; set; }
        public UserDto CreditedUser { get; set; }
        public UserDto DebitedUser { get; set; }
    }
}