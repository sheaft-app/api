using Sheaft.Interop.Enums;
using System;
using System.Collections.Generic;

namespace Sheaft.Models.Dto
{
    public class DocumentDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ValidationStatus Status { get; set; }
        public string ReasonCode { get; set; }
        public string ReasonMessage { get; set; }
        public IEnumerable<PageDto> Pages { get; set; }
    }
}