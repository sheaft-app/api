namespace Sheaft.Application.Common.Models.Dto
{
    public class PayoutDto : TransactionDto
    {
        public UserDto DebitedUser { get; set; }
    }
}