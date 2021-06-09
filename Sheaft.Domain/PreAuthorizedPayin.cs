using System;
using System.Linq;
using Sheaft.Core.Exceptions;
using Sheaft.Domain.Common;
using Sheaft.Domain.Enum;

namespace Sheaft.Domain
{
    public class PreAuthorizedPayin : Payin
    {        
        protected PreAuthorizedPayin(){}
        
        public PreAuthorizedPayin(Guid id, PreAuthorization preAuthorization, Wallet creditedWallet)
            : base(id, TransactionKind.PreAuthorizedPayin, creditedWallet, preAuthorization.Order)
        {
            var newPrices = GetPurchaseOrdersPrices(preAuthorization.Order);
            Fees = newPrices.FeesPrice;
            Debited = newPrices.TotalPrice;
        }
        
        private static OrderPrices GetPurchaseOrdersPrices(Order order)
        {
            var totalPrice = order.PurchaseOrders
                .Where(po => po.AcceptedOn.HasValue && !po.DroppedOn.HasValue)
                .Sum(po => po.TotalOnSalePrice);

            return Domain.Order.GetOrderFees(order, totalPrice);
        }
    }
}