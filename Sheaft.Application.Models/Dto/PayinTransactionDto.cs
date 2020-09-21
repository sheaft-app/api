namespace Sheaft.Application.Models
{
    public class PayinTransactionDto : TransactionDto
    {
        public UserProfileDto CreditedUser { get; set; }
        public OrderDto Order { get; set; }
    }
}