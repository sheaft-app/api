using System.Collections.Generic;

namespace Sheaft.Application.Common.Models.Dto
{
    public class StoresSearchDto
    {
        public long Count { get; set; }
        public IEnumerable<StoreDto> Stores { get; set; }
    }
}