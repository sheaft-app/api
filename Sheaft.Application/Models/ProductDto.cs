using System;
using System.Collections.Generic;
using Sheaft.Domain.Enum;

namespace Sheaft.Application.Models
{
    public class ProductDto
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
        public string ImageLarge { get; set; }
        public string ImageMedium { get; set; }
        public string ImageSmall { get; set; }
        public decimal QuantityPerUnit { get; set; }
        public UnitKind Unit { get; set; }
        public ConditioningKind Conditioning { get; set; }
        public decimal Vat { get; set; }
        public bool Available { get; set; }
        public bool VisibleToStores { get; set; }
        public bool VisibleToConsumers { get; set; }
        public int RatingsCount { get; set; }
        public decimal? Rating { get; set; }
        public bool IsReturnable { get; set; }
        public ReturnableDto Returnable { get; set; }
        public UserDto Producer { get; set; }
        public IEnumerable<TagDto> Tags { get; set; }
        public IEnumerable<RatingDto> Ratings { get; set; }
        public IEnumerable<ClosingDto> Closings { get; set; }
        public IEnumerable<PictureDto> Pictures { get; set; }
        public IEnumerable<CatalogPriceDto> Prices { get; set; }
    }
}
