using System;
using Sheaft.Domain.Enum;

namespace Sheaft.Domain
{
    public class WebPayin : Payin
    {
        protected WebPayin()
        {
        }

        public WebPayin(Guid id, Wallet creditedWallet, Order order)
            : base(id, TransactionKind.PayinWeb, creditedWallet, order)
        {
        }

        public string RedirectUrl { get; private set; }

        public void SetRedirectUrl(string url)
        {
            RedirectUrl = url;
        }
    }
}