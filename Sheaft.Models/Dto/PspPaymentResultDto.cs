using Sheaft.Interop.Enums;
using System;

namespace Sheaft.Models.Dto
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

    public class PspWebPaymentResultDto : PspPaymentResultDto
    {
        public string RedirectUrl { get; set; }
    }

    public class PspResultDto
    {
        public string Identifier { get; set; }
    }

    public class PspDocumentResultDto: PspResultDto
    {
        public DocumentStatus Status { get; set; }
        public string ResultCode { get; set; }
        public string ResultMessage { get; set; }
        public DateTimeOffset? ProcessedOn { get; set; }
    }

    public class PspDeclarationResultDto : PspResultDto
    {
        public DeclarationStatus Status { get; set; }
        public string ResultCode { get; set; }
        public string ResultMessage { get; set; }
        public DateTimeOffset? ProcessedOn { get; set; }
    }
}