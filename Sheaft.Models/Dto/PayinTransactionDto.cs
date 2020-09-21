using System;

namespace Sheaft.Models.Dto
{
    public class PayinTransactionDto : TransactionDto
    {
        public UserProfileDto CreditedUser { get; set; }
        public OrderDto Order { get; set; }
    }
}