using Sheaft.Domain.Enums;
using System;

namespace Sheaft.Domain.Models
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