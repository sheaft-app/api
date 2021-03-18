using System;
using Sheaft.Domain.Enum;

namespace Sheaft.Application.Models
{
    public abstract class LegalDto
    {
        public Guid Id { get; set; }
        public LegalValidation Validation { get; set; }
        public OwnerDto Owner { get; set; }
    }
}