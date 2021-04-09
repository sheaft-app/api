namespace Sheaft.Application.Models
{
    public class PayinDto : TransactionDto
    {
        public UserDto CreditedUser { get; set; }
    }
}