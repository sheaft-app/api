using Sheaft.Interop.Enums;
using System;
using System.Collections.Generic;

namespace Sheaft.Models.Dto
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
        public decimal WholeSalePricePerUnit { get; set; }
        public decimal VatPricePerUnit { get; set; }
        public decimal OnSalePricePerUnit { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }
        public string ImageLarge { get; set; }
        public string ImageMedium { get; set; }
        public string ImageSmall { get; set; }
        public decimal QuantityPerUnit { get; set; }
        public UnitKind Unit { get; set; }
        public decimal OnSalePrice { get; set; }
        public decimal WholeSalePrice { get; set; }
        public decimal VatPrice { get; set; }
        public decimal Vat { get; set; }
        public bool Available { get; set; }
        public int RatingsCount { get; set; }
        public decimal? Rating { get; set; }
        public bool IsReturnable { get; set; }
        public ReturnableDto Returnable { get; set; }
        public CompanyProfileDto Producer { get; set; }
        public IEnumerable<TagDto> Tags { get; set; }
        public IEnumerable<RatingDto> Ratings { get; set; }
    }
}