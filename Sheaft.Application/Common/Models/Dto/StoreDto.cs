using System.Collections.Generic;

namespace Sheaft.Application.Common.Models.Dto
{
    public class StoreDto : ProducerDto
    {
        public IEnumerable<TimeSlotDto> OpeningHours { get; set; }
    }
}