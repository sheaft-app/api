using System.Collections.Generic;

namespace Sheaft.Application.Common.Models.Inputs
{
    public class RegisterStoreInput : BusinessInput
    {
        public string SponsoringCode { get; set; }
        public CreateBusinessLegalInput Legals { get; set; }
        public IEnumerable<TimeSlotGroupInput> OpeningHours { get; set; }
    }
}