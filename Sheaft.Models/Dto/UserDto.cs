using Sheaft.Interop.Enums;
using System;

namespace Sheaft.Models.Dto
{
    public class UserDto : UserProfileDto
    {
        public AddressDto Address { get; set; }
    }
}