using System;
using System.Collections.Generic;
using Sheaft.Interop.Enums;
using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Commands
{
    public abstract class ProductCommand<T> : Command<T>
    {
        [JsonConstructor]
        protected ProductCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public string Reference { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
        public decimal WholeSalePricePerUnit { get; set; }
        public decimal QuantityPerUnit { get; set; }
        public UnitKind Unit { get; set; }
        public ConditioningKind Conditioning { get; set; }
        public decimal Vat { get; set; }
        public decimal? Weight { get; set; }
        public string Description { get; set; }
        public bool? Available { get; set; }
        public Guid? ReturnableId { get; set; }
        public IEnumerable<Guid> Tags { get; set; }
    }
}
