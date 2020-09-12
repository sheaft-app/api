using Sheaft.Interop.Enums;
using System;

namespace Sheaft.Domain.Models
{
    public class RefundTransferTransaction : Transaction
    {
        protected RefundTransferTransaction()
        {
        }

        public RefundTransferTransaction(Guid id, TransferTransaction transaction)
            : base(id, TransactionKind.RefundTransfer, transaction.Author)
        {
            TransactionToRefundIdentifier = transaction.Identifier;
            PurchaseOrder = transaction.PurchaseOrder;
        }

        public string TransactionToRefundIdentifier { get; private set; }
        public virtual PurchaseOrder PurchaseOrder { get; private set; }
    }
}