using System;
using System.Collections.Generic;

namespace Sheaft.Application.Models
{
    public class UpdateStoreInput : BusinessInput
    {
        public IEnumerable<TimeSlotGroupInput> OpeningHours { get; set; }
        public Guid Id { get; set; }
    }
}