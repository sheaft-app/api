using System;
using Sheaft.Domain.Enum;

namespace Sheaft.Application.Common.Models.ViewModels
{
    public class DocumentShortViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DocumentKind Kind { get; set; }
        public DateTimeOffset? ProcessedOn { get; private set; }
        public DocumentStatus Status { get; set; }
        public string ReasonCode { get; set; }
        public string ReasonMessage { get; set; }
    }
}
