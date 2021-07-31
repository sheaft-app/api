using System;
using System.Collections.Generic;

namespace Sheaft.Mailing
{
    public class ObservationMailerModel
    {
        public string ObservationId { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public string Comment { get; set; }
        public string PortalUrl { get; set; }
        public string Producer { get; set; }
        public string User { get; set; }
        public List<BatchMailerModel> Batches { get; set; }
        public List<ProductMailerModel> Products { get; set; }
        public string ProducerId { get; set; }
    }
}