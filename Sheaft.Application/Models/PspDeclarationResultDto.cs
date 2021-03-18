using System;
using Sheaft.Domain.Enum;

namespace Sheaft.Application.Models
{
    public class PspDeclarationResultDto : PspResultDto
    {
        public DeclarationStatus Status { get; set; }
        public string ResultCode { get; set; }
        public string ResultMessage { get; set; }
        public DateTimeOffset? ProcessedOn { get; set; }
    }
}