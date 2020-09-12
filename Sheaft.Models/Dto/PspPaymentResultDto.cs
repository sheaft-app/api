using Sheaft.Interop.Enums;
using System;

namespace Sheaft.Models.Dto
{
    public class PspPaymentResultDto
    {
        public string Identifier { get; set; }
        public TransactionStatus Status { get; set; }
        public string ResultCode { get; set; }
        public string ResultMessage { get; set; }
        public decimal Credited { get; set; }
        public decimal Debited { get; set; }
        public decimal Fees { get; set; }
        public DateTimeOffset? ExecutedOn { get; set; }
    }

    public class PspWebPaymentResultDto : PspPaymentResultDto
    {
        public string RedirectUrl { get; set; }
    }
}