using System;
using System.Collections.Generic;
using Sheaft.Domain.Enum;

namespace Sheaft.Application.Common.Models.Dto
{
    public class DeclarationDto
    {
        public Guid Id { get; private set; }
        public DeclarationStatus Status { get; set; }
        public string ReasonCode { get; set; }
        public string ReasonMessage { get; set; }
        public IEnumerable<UboDto> Ubos { get; set; }
    }
}