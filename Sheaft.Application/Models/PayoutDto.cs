namespace Sheaft.Application.Models
{
    public class PayoutDto : TransactionDto
    {
        public UserDto DebitedUser { get; set; }
        public BankAccountDto BankAccount { get; set; }
    }
}