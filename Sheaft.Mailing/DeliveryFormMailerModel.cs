using System;
using System.Collections.Generic;

namespace Sheaft.Mailing
{
    public class DeliveryFormMailerModel : DeliveryReceiptMailerModel
    {
        public List<DeliveryProductMailerModel> ReturnedProducts { get; set; }
        public List<DeliveryReturnableMailerModel> ReturnedReturnables { get; set; }
        public DateTimeOffset? DeliveredOn { get; set; }
    }
}