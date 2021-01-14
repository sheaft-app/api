using Sheaft.Domain.Enums;
using System;

namespace Sheaft.Domain.Models
{
    public class PreAuthorizedPayin : Payin
    {
        protected PreAuthorizedPayin()
        {
        }

        public PreAuthorizedPayin(Guid id, PreAuthorization preAuthorization, Wallet creditedWallet)
            : base(id, TransactionKind.PreAuthorizedPayin, creditedWallet, preAuthorization.Order)
        {
            PreAuthorization = preAuthorization;
        }
        
        public virtual PreAuthorization PreAuthorization { get; }
    }
}