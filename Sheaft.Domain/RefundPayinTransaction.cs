using Sheaft.Interop.Enums;
using System;

namespace Sheaft.Domain.Models
{
    public class RefundPayinTransaction : Transaction
    {
        protected RefundPayinTransaction()
        {
        }

        public RefundPayinTransaction(Guid id, RefundPayinTransaction transaction)
            : base(id, TransactionKind.Payin, TransactionNature.Refund, transaction.Author)
        {
            TransactionToRefundIdentifier = transaction.Identifier;
            Debited = transaction.Debited;
            Order = transaction.Order;
        }

        public string TransactionToRefundIdentifier { get; private set; }
        public virtual Order Order { get; private set; }
    }
}