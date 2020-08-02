using System;
using System.Collections.Generic;

namespace Sheaft.Models.Dto
{
    public class QuickOrderDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsDefault { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public DateTimeOffset? RemovedOn { get; set; }
        public UserProfileDto User { get; set; }
        public IEnumerable<QuickOrderProductQuantityDto> Products { get; set; }
    }
}