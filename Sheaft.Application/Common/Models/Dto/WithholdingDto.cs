namespace Sheaft.Application.Common.Models.Dto
{
    public class WithholdingDto : TransactionDto
    {
        public UserDto CreditedUser { get; set; }
    }
}