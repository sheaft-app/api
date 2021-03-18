namespace Sheaft.Application.Models
{
    public class DonationDto : TransactionDto
    {
        public UserDto CreditedUser { get; set; }
    }
}