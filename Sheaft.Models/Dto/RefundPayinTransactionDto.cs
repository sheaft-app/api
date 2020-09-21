using System;

namespace Sheaft.Models.Dto
{

    public class RefundPayinTransactionDto : RefundTransactionDto
    {
        public UserProfileDto DebitedUser { get; set; }
    }
}