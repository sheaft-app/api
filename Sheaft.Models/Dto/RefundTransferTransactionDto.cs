using System;

namespace Sheaft.Models.Dto
{

    public class RefundTransferTransactionDto : RefundTransactionDto
    {
        public UserProfileDto DebitedUser { get; set; }
        public UserProfileDto CreditedUser { get; set; }
    }
}