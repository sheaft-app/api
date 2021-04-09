using System;
using Sheaft.Domain.Enum;

namespace Sheaft.Domain
{
    public class PreAuthorizedPayin : Payin
    {        
        protected PreAuthorizedPayin(){}
        
        public PreAuthorizedPayin(Guid id, PreAuthorization preAuthorization, Wallet creditedWallet)
            : base(id, TransactionKind.PreAuthorizedPayin, creditedWallet, preAuthorization.Order)
        {
            PreAuthorization = preAuthorization;
        }
        
        public virtual PreAuthorization PreAuthorization { get; private set;}
    }
}