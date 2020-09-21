using System;

namespace Sheaft.Models.Dto
{
    public class RefundTransactionDto : TransactionDto
    {
        public TransactionDto RefundedTransaction { get; set; }
    }
}