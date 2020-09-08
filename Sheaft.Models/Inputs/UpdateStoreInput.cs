using System;
using System.Collections.Generic;

namespace Sheaft.Models.Inputs
{
    public class UpdateStoreInput : CompanyInput
    {
        public IEnumerable<TimeSlotGroupInput> OpeningHours { get; set; }
        public Guid Id { get; set; }
    }
}