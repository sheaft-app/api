using System;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain
{
    public class ObservationProduct : IIdEntity, ITrackCreation
    { 
        protected ObservationProduct()
        {
        }
        
        public ObservationProduct(Product product)
        {
            Id = Guid.NewGuid();
            ProductId = product.Id;
            Name = product.Name;
            Reference = product.Reference;
            Conditioning = product.Conditioning;
            Unit = product.Unit;
            Vat = product.Vat;
            Picture = product.Picture;
            QuantityPerUnit = product.QuantityPerUnit;
        }

        public Guid Id { get; private set; }
        public DateTimeOffset CreatedOn { get; private set; }
        public string Name { get; private set; }
        public string Reference { get; private set; }
        public UnitKind Unit { get; private set; }
        public decimal Vat { get; private set; }
        public decimal QuantityPerUnit { get; private set; }
        public ConditioningKind Conditioning { get; private set; }
        public string Picture { get; private set; }
        public Guid ProductId { get; private set; }
        public Guid ObservationId { get; private set; }
        public virtual Product Product { get; set; }
    }
}