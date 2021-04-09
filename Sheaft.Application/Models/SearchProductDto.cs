using System;
using System.Collections.Generic;
using Sheaft.Domain.Enum;

namespace Sheaft.Application.Models
{
    public class SearchProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal OnSalePricePerUnit { get; set; }
        public decimal OnSalePrice { get; set; }
        public string Picture { get; set; }
        public string ImageLarge { get; set; }
        public string ImageMedium { get; set; }
        public string ImageSmall { get; set; }
        public decimal QuantityPerUnit { get; set; }
        public UnitKind Unit { get; set; }
        public ConditioningKind Conditioning { get; set; }
        public bool Available { get; set; }
        public int RatingsCount { get; set; }
        public decimal? Rating { get; set; }
        public bool IsReturnable { get; set; }
        public UserDto Producer { get; set; }
        public IEnumerable<TagDto> Tags { get; set; }
    }
}