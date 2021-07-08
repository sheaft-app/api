using System;
using Sheaft.Domain.Enum;

namespace Sheaft.Web.Manage.Models
{
    public class ShortProductViewModel
    {
        public Guid Id { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public DateTimeOffset? RemovedOn { get; set; }
        public string Reference { get; set; }
        public string Name { get; set; }
        public decimal? Weight { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }
        public UnitKind Unit { get; set; }
        public decimal QuantityPerUnit { get; set; }
        public ConditioningKind Conditioning { get; set; }
        public decimal Vat { get; set; }
        public bool Available { get; set; }
        public int RatingsCount { get; set; }
        public int TagsCount { get; set; }
        public int PicturesCount { get; set; }
        public int CatalogsPricesCount { get; set; }
        public VisibleToKind VisibleTo { get; set; }
        public decimal? Rating { get; set; }

        public Guid? ReturnableId { get; set; }

        public Guid ProducerId { get; set; }
        public virtual ReturnableViewModel Returnable { get; set; }
        public virtual ProducerViewModel Producer { get; set; }
    }
}