using System;

namespace Sheaft.Mailing
{
    public class DeliveryBatchMailerModel
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public DateTimeOffset ScheduledOn { get; set; }
        public string Firstname { get; set; }
        public string ProducerName { get; set; }
    }
}