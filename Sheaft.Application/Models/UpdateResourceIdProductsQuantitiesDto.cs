using System;
using System.Collections.Generic;

namespace Sheaft.Application.Models
{
    public class UpdateResourceIdProductsQuantitiesDto
    {
        public Guid Id { get; set; }
        public IEnumerable<ResourceIdQuantityDto> Products { get; set; }
    }
}