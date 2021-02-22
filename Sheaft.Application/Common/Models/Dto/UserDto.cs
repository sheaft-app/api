namespace Sheaft.Application.Common.Models.Dto
{
    public class UserDto : UserProfileDto
    {
        public AddressDto Address { get; set; }
        public ProfileInformationDto ProfileInformation { get; set; }
    }
}