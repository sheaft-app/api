using System;
using System.Collections.Generic;

namespace Sheaft.Mailing
{
    public class RecallMailerModel
    {
        public DateTimeOffset CreatedOn { get; set; }
        public string Comment { get; set; }
        public string PortalUrl { get; set; }
        public string ProducerName { get; set; }
        public string User { get; set; }
        public DateTimeOffset SaleStartedOn { get; set; }
        public DateTimeOffset SaleEndedOn { get; set; }
        public List<BatchMailerModel> Batches { get; set; }
        public List<ProductMailerModel> Products { get; set; }
        public string RecallId { get; set; }
    }
}