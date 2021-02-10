using System;
using System.Collections.Generic;

namespace Sheaft.Application.Common.Models.Inputs
{
    public class UpdateIdProductsQuantitiesInput
    {
        public Guid Id { get; set; }
        public IEnumerable<ProductQuantityInput> Products { get; set; }
    }
}