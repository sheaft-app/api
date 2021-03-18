using System;
using Sheaft.Domain.Enum;

namespace Sheaft.Application.Models
{
    public class PspDocumentResultDto : PspResultDto
    {
        public DocumentStatus Status { get; set; }
        public string ResultCode { get; set; }
        public string ResultMessage { get; set; }
        public DateTimeOffset? ProcessedOn { get; set; }
    }
}