using System.Collections.Generic;

namespace Sheaft.Models.Inputs
{
    public class RegisterStoreInput : BusinessInput
    {
        public string SponsoringCode { get; set; }
        public IEnumerable<TimeSlotGroupInput> OpeningHours { get; set; }
    }
}