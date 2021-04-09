namespace Sheaft.Application.Models
{
    public class PayinDto : TransactionDto
    {
        public string RedirectUrl { get; set; }
        public UserDto CreditedUser { get; set; }
    }
}