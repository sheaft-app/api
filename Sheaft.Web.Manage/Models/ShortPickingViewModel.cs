using System;
using Sheaft.Domain.Enum;

namespace Sheaft.Web.Manage.Models
{
    public class ShortPickingViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public DateTimeOffset? RemovedOn { get; set; }
        public PickingStatus Status { get; set; }
        public DateTimeOffset? StartedOn { get; set; }
        public DateTimeOffset? CompletedOn { get; set; }
        public int PurchaseOrdersCount { get; set; }
        public int ProductsToPrepareCount { get; set; }
        public int PreparedProductsCount { get; set; }
        public string PickingFormUrl { get; set; }
        public Guid ProducerId { get; set; }
        public UserViewModel Producer { get; set; }
    }
}