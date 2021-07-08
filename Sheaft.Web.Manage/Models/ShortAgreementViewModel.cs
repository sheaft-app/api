using System;
using Sheaft.Domain.Enum;

namespace Sheaft.Web.Manage.Models
{
    public class ShortAgreementViewModel
    {
        public Guid Id { get; set; }
        public AgreementStatus Status { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public DateTimeOffset? RemovedOn { get; set; }
        public Guid? DeliveryModeId { get; set; }
        public Guid? CatalogId { get; set; }
        public StoreViewModel Store { get; set; }
        public ProducerViewModel Producer { get; set; }
        public int Position { get; set; }
        public string Reason { get; set; }
    }
}