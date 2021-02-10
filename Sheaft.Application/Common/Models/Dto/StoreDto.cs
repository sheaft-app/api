using System.Collections.Generic;

namespace Sheaft.Application.Models
{
    public class StoreDto : ProducerDto
    {
        public IEnumerable<TimeSlotDto> OpeningHours { get; set; }
    }
}