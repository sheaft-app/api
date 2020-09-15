using Sheaft.Interop.Enums;
using System;
using System.Collections.Generic;

namespace Sheaft.Models.Dto
{
    public class UboDeclarationDto
    {
        public Guid Id { get; private set; }
        public DeclarationStatus Status { get; set; }
        public string ReasonCode { get; set; }
        public string ReasonMessage { get; set; }
        public IEnumerable<UboDto> Ubos { get; set; }
    }
}