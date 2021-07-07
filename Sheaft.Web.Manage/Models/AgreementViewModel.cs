using System;
using System.Collections.Generic;
using Sheaft.Domain.Enum;

namespace Sheaft.Web.Manage.Models
{
    public class AgreementViewModel
    {
        public Guid Id { get; set; }
        public AgreementStatus Status { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public DateTimeOffset? RemovedOn { get; set; }
        public string Reason { get; set; }
        public Guid? DeliveryModeId { get; set; }
        public DeliveryModeViewModel DeliveryMode { get; set; }
        public Guid? CatalogId { get; set; }
        public CatalogViewModel Catalog { get; set; }
        public StoreViewModel Store { get; set; }
        public ProducerViewModel Producer { get; set; }
    }
}
