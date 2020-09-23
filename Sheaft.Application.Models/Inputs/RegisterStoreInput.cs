using System.Collections.Generic;

namespace Sheaft.Application.Models
{
    public class RegisterStoreInput : BusinessInput
    {
        public string SponsoringCode { get; set; }
        public BusinessLegalInput Legals { get; set; }
        public IEnumerable<TimeSlotGroupInput> OpeningHours { get; set; }
    }
}