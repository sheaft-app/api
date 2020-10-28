using Sheaft.Domain.Enums;
using Sheaft.Exceptions;
using System;

namespace Sheaft.Domain.Models
{
    public abstract class Payin : Transaction
    {
        protected Payin()
        {
        }

        protected Payin(Guid id, TransactionKind kind, Wallet creditedWallet, Order order)
            : base(id, kind, creditedWallet.User)
        {
            Order = order;
            Fees = order.FeesPrice;
            Debited = order.TotalPrice;
            CreditedWallet = creditedWallet;
            Reference = "SHEAFT";
        }

        public virtual Wallet CreditedWallet { get; private set; }
        public virtual Order Order { get; private set; }
        public virtual PayinRefund Refund { get; private set; }

        public void SetRefund(PayinRefund refund)
        {
            if (Refund != null && Refund.Status == TransactionStatus.Succeeded)
                throw new ValidationException(MessageKind.Payin_CannotSet_Refund_AlreadySucceeded);

            Refund = null;
            Refund = refund;
        }
    }
}