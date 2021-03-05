using System.Collections.Generic;

namespace Sheaft.Application.Common.Models.Dto
{
    public class StoreDto : UserDto
    {
        public string VatIdentifier { get; set; }
        public string Siret { get; set; }
        public bool OpenForNewBusiness { get; set; }
        public IEnumerable<TagDto> Tags { get; set; }
        public IEnumerable<TimeSlotDto> OpeningHours { get; set; }
    }
}