using Sheaft.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Sheaft.Application.Models
{
    public class UboDeclarationViewModel
    {
        public Guid Id { get; private set; }
        public DeclarationStatus Status { get; set; }
        public string ReasonCode { get; set; }
        public string ReasonMessage { get; set; }
        public IEnumerable<UboViewModel> Ubos { get; set; }
    }
}
