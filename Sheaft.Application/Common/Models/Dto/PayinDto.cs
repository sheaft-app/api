namespace Sheaft.Application.Common.Models.Dto
{
    public class PayinDto : TransactionDto
    {
        public UserProfileDto CreditedUser { get; set; }
        public OrderDto Order { get; set; }
    }
}