namespace Sheaft.Application.Models
{
    public class PayinDto : TransactionDto
    {
        public UserProfileDto CreditedUser { get; set; }
        public OrderDto Order { get; set; }
    }
}