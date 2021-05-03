using System.Collections.Generic;

namespace Sheaft.Application.Models
{
    public class StoresSearchDto
    {
        public long Count { get; set; }
        public IEnumerable<UserDto> Stores { get; set; }
    }
}