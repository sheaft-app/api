using Sheaft.Domain.Enums;
using System;

namespace Sheaft.Application.Models
{
    public class PspPaymentResultDto: PspResultDto
    {
        public TransactionStatus Status { get; set; }
        public string ResultCode { get; set; }
        public string ResultMessage { get; set; }
        public decimal Credited { get; set; }
        public decimal Debited { get; set; }
        public decimal Fees { get; set; }
        public DateTimeOffset? ExecutedOn { get; set; }
    }
}