using System;

namespace Sheaft.Models.Dto
{
    public class PayinTransactionDto : BaseTransactionDto
    {
        public decimal Fees { get; set; }
        public decimal Credited { get; set; }
        public UserProfileDto CreditedUser { get; set; }
        public OrderDto Order { get; set; }
    }
}