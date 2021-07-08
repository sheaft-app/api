using System.Collections.Generic;

namespace Sheaft.Web.Manage.Models
{
    public class OrderViewModel : ShortOrderViewModel
    {
        public IEnumerable<ShortPurchaseOrderViewModel> PurchaseOrders { get; set; }
    }
}
