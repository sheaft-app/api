using System.Collections.Generic;

namespace Sheaft.Web.Manage.Models
{
    public class DeliveryViewModel : ShortDeliveryViewModel
    {
        public virtual ICollection<ShortPurchaseOrderViewModel> PurchaseOrders { get; set; }
        public virtual ICollection<DeliveryProductViewModel> Products { get; set; }
        public virtual ICollection<DeliveryReturnableViewModel> ReturnedReturnables { get; set; }
    }
}