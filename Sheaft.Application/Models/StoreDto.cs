using System.Collections.Generic;

namespace Sheaft.Application.Models
{
    public class StoreDto : UserDto
    {
        public string VatIdentifier { get; set; }
        public string Siret { get; set; }
        public bool OpenForNewBusiness { get; set; }
        public IEnumerable<TagDto> Tags { get; set; }
        public IEnumerable<TimeSlotDto> OpeningHours { get; set; }
        public IEnumerable<ClosingDto> Closings { get; set; }
        public IEnumerable<PictureDto> Pictures { get; set; }
    }
}