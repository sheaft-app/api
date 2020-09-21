using Sheaft.Domain.Enums;
using System;

namespace Sheaft.Domain.Models
{
    public class RefundPayinTransaction : RefundTransaction
    {
        protected RefundPayinTransaction()
        {
        }

        public RefundPayinTransaction(Guid id, PayinTransaction transaction, User author)
            : base(id, TransactionKind.RefundPayin, transaction, author)
        {
            Credited = transaction.Debited;
            PayinTransaction = transaction;
        }

        public virtual PayinTransaction PayinTransaction { get; private set; }
    }
}