using System.Collections.Generic;

namespace Sheaft.Application.Common.Models.Dto
{
    public class PayoutDto : TransactionDto
    {
        public UserDto DebitedUser { get; set; }
        public BankAccountDto BankAccount { get; set; }
    }
}