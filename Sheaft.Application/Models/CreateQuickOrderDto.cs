using System;
using System.Collections.Generic;

namespace Sheaft.Application.Models
{
    public class CreateQuickOrderDto
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<ResourceIdQuantityDto> Products { get; set; }
    }
}