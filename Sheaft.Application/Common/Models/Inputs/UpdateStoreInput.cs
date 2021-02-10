using System;
using System.Collections.Generic;

namespace Sheaft.Application.Models
{
    public class UpdateStoreInput : BusinessInput
    {
        public Guid Id { get; set; }
        public IEnumerable<TimeSlotGroupInput> OpeningHours { get; set; }
    }
}