using System;
using Sheaft.Domain.Enum;

namespace Sheaft.Application.Common.Models.Dto
{
    public class PspDocumentResultDto : PspResultDto
    {
        public DocumentStatus Status { get; set; }
        public string ResultCode { get; set; }
        public string ResultMessage { get; set; }
        public DateTimeOffset? ProcessedOn { get; set; }
    }
}