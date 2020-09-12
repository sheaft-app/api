using Sheaft.Interop.Enums;
using System;

namespace Sheaft.Models.Dto
{
    public class BaseTransactionDto
    {
        public Guid Id { get; set; }
        public string Identifier { get; set; }
        public TransactionKind Kind { get; set; }
        public TransactionStatus Status { get; set; }
        public string Reference { get; set; }
        public string ResultCode { get; set; }
        public string ResultMessage { get; set; }
    }
}