using Sheaft.Domain.Enums;
using Sheaft.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sheaft.Domain.Models
{
    [Obsolete("You must use the PreAuthorization feature")]
    public class WebPayin : Transaction
    {
        private List<WebPayinRefund> _refunds;

        protected WebPayin()
        {
        }

        protected WebPayin(Guid id, Wallet creditedWallet, Order order)
            : base(id, TransactionKind.WebPayin, creditedWallet.User)
        {
            Order = order;
            Fees = order.FeesPrice;
            Debited = order.TotalPrice;
            CreditedWallet = creditedWallet;
            Reference = $"SHFT{DateTime.UtcNow.ToString("DDMMYY")}";

            _refunds = new List<WebPayinRefund>();
        }

        public string RedirectUrl { get; private set; }
        public virtual Order Order { get; private set; }
        public virtual Wallet CreditedWallet { get; private set; }
        public virtual IReadOnlyCollection<WebPayinRefund> Refunds => _refunds.AsReadOnly();

        public void AddRefund(WebPayinRefund refund)
        {
            if (Refunds != null && Refunds.Any(r => r.PurchaseOrder.Id == refund.PurchaseOrder.Id && r.Status == TransactionStatus.Succeeded))
                throw new ValidationException(MessageKind.Payin_CannotAdd_Refund_PurchaseOrderRefund_AlreadySucceeded);

            _refunds.Add(refund);
        }

        public void SetRedirectUrl(string url)
        {
            RedirectUrl = url;
        }
    }
}