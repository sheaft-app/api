using Sheaft.Domain.Enums;
using System;

namespace Sheaft.Domain.Models
{
    public class PreAuthorizedDonationPayin : PreAuthorizedPayin
    {
        protected PreAuthorizedDonationPayin()
        {
        }

        public PreAuthorizedDonationPayin(Guid id, PreAuthorization preAuthorization, Wallet creditedWallet)
            : base(id, TransactionKind.PreAuthorizedDonationPayin, preAuthorization, creditedWallet)
        {       
            Debited = Math.Round(preAuthorization.Order.Donate - preAuthorization.Order.InternalFeesPrice, 2);
        }
    }
}