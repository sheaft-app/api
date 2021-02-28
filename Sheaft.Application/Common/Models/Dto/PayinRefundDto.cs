namespace Sheaft.Application.Common.Models.Dto
{
    public class PayinRefundDto : TransactionDto
    {
        public UserDto DebitedUser { get; set; }
        public TransactionDto Payin { get; set; }
    }
}