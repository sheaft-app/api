using System.Collections.Generic;

namespace Sheaft.Web.Manage.Models
{
    public class PurchaseOrderViewModel : ShortPurchaseOrderViewModel
    {
        public TransferInfoViewModel Transfer { get; set; }
        public IEnumerable<PurchaseOrderProductViewModel> Products { get; set; }

    }
}
