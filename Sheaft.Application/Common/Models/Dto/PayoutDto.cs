namespace Sheaft.Application.Common.Models.Dto
{
    public class PayoutDto : TransactionDto
    {
        public UserProfileDto DebitedUser { get; set; }
    }
}