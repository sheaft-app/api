﻿using System;
using System.Collections.Generic;
using Sheaft.Interop.Enums;

namespace Sheaft.Application.Commands
{
    public abstract class ProductCommand<T> : Command<T>
    {
        protected ProductCommand(Interop.IRequestUser user) : base(user)
        {
        }

        public string Reference { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
        public decimal WholeSalePricePerUnit { get; set; }
        public decimal QuantityPerUnit { get; set; }
        public UnitKind Unit { get; set; }
        public decimal Vat { get; set; }
        public decimal? Weight { get; set; }
        public string Description { get; set; }
        public bool? Available { get; set; }
        public Guid? PackagingId { get; set; }
        public IEnumerable<Guid> Tags { get; set; }
    }
}
