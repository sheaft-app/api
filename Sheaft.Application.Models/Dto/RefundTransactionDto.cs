using System;

namespace Sheaft.Application.Models
{
    public class RefundTransactionDto : TransactionDto
    {
        public TransactionDto RefundedTransaction { get; set; }
    }
}