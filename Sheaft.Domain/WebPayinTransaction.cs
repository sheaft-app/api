using Sheaft.Interop.Enums;
using System;

namespace Sheaft.Domain.Models
{
    public class WebPayinTransaction : Transaction
    {
        protected WebPayinTransaction()
        {
        }

        public WebPayinTransaction(Guid id, Wallet creditedWallet, Order order)
            : base(id, TransactionKind.PayinWeb, creditedWallet.User)
        {
            Order = order;
            Fees = order.Fees;
            Debited = order.TotalWholeSalePrice + order.Fees;
            CreditedWallet = creditedWallet;
            Reference = $"SHEAFT_{DateTimeOffset.UtcNow:yyyyMMdd}";
        }

        public string RedirectUrl { get; private set; }
        public virtual Order Order { get; private set; }

        public void SetRedirectUrl(string url)
        {
            RedirectUrl = url;
        }
    }
}