namespace Sheaft.Models.Dto
{
    public class UserDto : UserProfileDto
    {
        public AddressDto Address { get; set; }
        public AddressDto BillingAddress { get; set; }
    }
}