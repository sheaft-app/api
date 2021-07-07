using System;
using System.Collections.Generic;

namespace Sheaft.Mailing
{
    public class DeliveryFormMailerModel : DeliveryReceiptMailerModel
    {
        public List<DeliveryProductMailerModel> ProductsDiffs { get; set; }
        public List<DeliveryReturnableMailerModel> ReturnedReturnables { get; set; }
        public List<DeliveryReturnableMailerModel> ReturnablesDiffs { get; set; }
        public DateTimeOffset? DeliveredOn { get; set; }
    }
}