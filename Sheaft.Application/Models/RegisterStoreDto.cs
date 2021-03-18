using System.Collections.Generic;

namespace Sheaft.Application.Models
{
    public class RegisterStoreDto : BusinessDto
    {
        public string SponsoringCode { get; set; }
        public CreateBusinessLegalDto Legals { get; set; }
        public IEnumerable<TimeSlotGroupDto> OpeningHours { get; set; }
    }
}