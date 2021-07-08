using System.Collections.Generic;

namespace Sheaft.Web.Manage.Models
{
    public class DeliveryBatchViewModel : ShortDeliveryBatchViewModel
    {
        public virtual ICollection<ShortDeliveryViewModel> Deliveries { get; set; }
    }
}