using System;

namespace Sheaft.Models.Dto
{
    public class PayoutTransactionDto : BaseTransactionDto
    {
        public decimal Fees { get; set; }
        public decimal Debited { get; set; }
        public UserProfileDto DebitedUser { get; set; }
    }
}