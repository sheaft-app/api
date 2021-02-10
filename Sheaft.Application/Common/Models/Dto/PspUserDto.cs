using Sheaft.Domain.Enums;
using System;

namespace Sheaft.Application.Models
{

    public abstract class PspUserDto
    {
        public string Email { get; set; }
        public LegalValidation KYCLevel { get; set; }
    }
}