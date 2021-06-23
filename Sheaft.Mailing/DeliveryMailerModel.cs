using System;

namespace Sheaft.Mailing
{
    public class DeliveryMailerModel
    {
        public string ProducerName { get; set; }
        public string Url { get; set; }
        public DateTimeOffset? DeliveredOn { get; set; }
        public DateTimeOffset ScheduledOn { get; set; }
    }
}