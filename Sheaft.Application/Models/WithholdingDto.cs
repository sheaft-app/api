namespace Sheaft.Application.Models
{
    public class WithholdingDto : TransactionDto
    {
        public UserDto CreditedUser { get; set; }
    }
}