using System.Collections.Generic;

namespace Sheaft.Models.Dto
{
    public class StoreDto : ProducerDto
    {
        public IEnumerable<TimeSlotDto> OpeningHours { get; set; }
    }
}