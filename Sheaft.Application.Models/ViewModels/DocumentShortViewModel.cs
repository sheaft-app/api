using Sheaft.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Sheaft.Application.Models
{
    public class DocumentShortViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DocumentKind Kind { get; set; }
        public DocumentStatus Status { get; set; }
        public string ReasonCode { get; set; }
        public string ReasonMessage { get; set; }
    }
}
