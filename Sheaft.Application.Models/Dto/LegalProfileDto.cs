using Sheaft.Domain.Enums;
using System;

namespace Sheaft.Application.Models
{
    public class BaseLegalDto
    {
        public Guid Id { get; set; }
        public LegalValidation Validation { get; set; }
        public OwnerDto Owner { get; set; }
    }
}