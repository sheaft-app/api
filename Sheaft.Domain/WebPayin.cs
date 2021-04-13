using System;
using System.Collections.Generic;
using System.Linq;
using Sheaft.Core.Enums;
using Sheaft.Core.Exceptions;
using Sheaft.Domain.Common;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Events.Payin;

namespace Sheaft.Domain
{
    [Obsolete("You must use the PreAuthorizationPayin")]
    public class WebPayin : Payin, IHasDomainEvent
    {
        protected WebPayin()
        {
        }

        public WebPayin(Guid id, Wallet creditedWallet, Order order)
            : base(id, TransactionKind.WebPayin, creditedWallet, order)
        {
        }

        public string RedirectUrl { get; private set; }

        public void SetRedirectUrl(string url)
        {
            RedirectUrl = url;
        }
    }
}