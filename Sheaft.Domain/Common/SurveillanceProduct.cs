using System;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain.BaseClass
{
    public abstract class SurveillanceProduct : ITrackUpdate
    { 
        protected SurveillanceProduct()
        {
        }
        
        protected SurveillanceProduct(Product product)
        {
            ProductId = product.Id;
            Name = product.Name;
            Reference = product.Reference;
            Conditioning = product.Conditioning;
            Unit = product.Unit;
            Picture = product.Picture;
            QuantityPerUnit = product.QuantityPerUnit;
        }

        public Guid Id { get; } = Guid.NewGuid();
        public DateTimeOffset UpdatedOn { get; private set; }
        public string Name { get; private set; }
        public string Reference { get; private set; }
        public UnitKind Unit { get; private set; }
        public decimal QuantityPerUnit { get; private set; }
        public ConditioningKind Conditioning { get; private set; }
        public string Picture { get; private set; }
        public Guid ProductId { get; private set; }
    }
}