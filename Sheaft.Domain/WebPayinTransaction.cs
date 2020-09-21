using Sheaft.Domain.Enums;
using System;

namespace Sheaft.Domain.Models
{
    public class WebPayinTransaction : PayinTransaction
    {
        protected WebPayinTransaction()
        {
        }

        public WebPayinTransaction(Guid id, Wallet creditedWallet, Order order)
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