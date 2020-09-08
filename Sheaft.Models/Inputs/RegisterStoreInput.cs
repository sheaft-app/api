using System.Collections.Generic;

namespace Sheaft.Models.Inputs
{
    public class RegisterStoreInput : CompanyInput
    {
        public IEnumerable<TimeSlotGroupInput> OpeningHours { get; set; }
        public RegisterOwnerInput Owner { get; set; }
    }
}