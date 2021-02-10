using Sheaft.Domain.Enums;
using System;

namespace Sheaft.Application.Models
{
    public class PspTransactionResultDto : PspResultDto
    {
        public TransactionStatus Status { get; set; }
        public string ResultCode { get; set; }
        public string ResultMessage { get; set; }
        public DateTimeOffset? ProcessedOn { get; set; }
    }
}