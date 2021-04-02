using System;
using System.Collections.Generic;

namespace Sheaft.Application.Models
{
    public class UpdateStoreDto : BusinessDto
    {
        public Guid Id { get; set; }
        public IEnumerable<TimeSlotGroupDto> OpeningHours { get; set; }
    }
}