using Sheaft.Domain.Enums;
using System;

namespace Sheaft.Domain.Models
{
    public abstract class PreAuthorizedPayin : Transaction
    {        
        protected PreAuthorizedPayin(){}
        
        protected PreAuthorizedPayin(Guid id, TransactionKind kind, PreAuthorization preAuthorization, Wallet creditedWallet)
            : base(id, kind, creditedWallet.User)
        {
            CreditedWallet = creditedWallet;
            PreAuthorization = preAuthorization;
        }

        public virtual Wallet CreditedWallet { get; private set; }
        public virtual PreAuthorization PreAuthorization { get; private set;}
    }
}