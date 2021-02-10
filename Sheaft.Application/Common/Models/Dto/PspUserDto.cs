using Sheaft.Domain.Enum;

namespace Sheaft.Application.Common.Models.Dto
{

    public abstract class PspUserDto
    {
        public string Email { get; set; }
        public LegalValidation KYCLevel { get; set; }
    }
}