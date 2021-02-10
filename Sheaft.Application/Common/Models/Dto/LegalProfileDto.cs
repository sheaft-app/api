using System;
using Sheaft.Domain.Enum;

namespace Sheaft.Application.Common.Models.Dto
{
    public class BaseLegalDto
    {
        public Guid Id { get; set; }
        public LegalValidation Validation { get; set; }
        public OwnerDto Owner { get; set; }
    }
}