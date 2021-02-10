namespace Sheaft.Application.Common.Models.Dto
{
    public class PayinRefundDto : TransactionDto
    {
        public UserProfileDto DebitedUser { get; set; }
        public TransactionDto Payin { get; set; }
    }
}