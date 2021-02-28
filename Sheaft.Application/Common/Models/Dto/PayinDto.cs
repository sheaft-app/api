namespace Sheaft.Application.Common.Models.Dto
{
    public class PayinDto : TransactionDto
    {
        public UserDto CreditedUser { get; set; }
    }
}