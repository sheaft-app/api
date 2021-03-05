namespace Sheaft.Application.Common.Models.Dto
{
    public class DonationDto : TransactionDto
    {
        public UserDto CreditedUser { get; set; }
    }
}