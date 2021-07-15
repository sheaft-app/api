using System.Collections.Generic;

namespace Sheaft.Web.Manage.Models
{
    public class PickingViewModel : ShortPickingViewModel
    {
        public List<ShortPurchaseOrderViewModel> PurchaseOrders { get; set; }
        public List<PickingProductViewModel> ProductsToPrepare { get; set; }
        public List<PreparedProductViewModel> PreparedProducts { get; set; }
    }
}