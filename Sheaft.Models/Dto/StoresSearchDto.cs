using Sheaft.Interop.Enums;
using System;
using System.Collections.Generic;

namespace Sheaft.Models.Dto
{

    public class StoresSearchDto
    {
        public long Count { get; set; }
        public IEnumerable<StoreDto> Stores { get; set; }
    }
}