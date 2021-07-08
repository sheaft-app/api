using System;
using System.Collections.Generic;
using Sheaft.Domain.Enum;

namespace Sheaft.Web.Manage.Models
{
    public class DeliveryViewModel : ShortDeliveryViewModel
    {
        public virtual ICollection<ShortPurchaseOrderViewModel> PurchaseOrders { get; set; }
        public virtual ICollection<DeliveryProductViewModel> Products { get; set; }
        public virtual ICollection<DeliveryReturnableViewModel> ReturnedReturnables { get; set; }
    }
}
