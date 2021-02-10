using Sheaft.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Sheaft.Application.Models
{
    public class StoresSearchDto
    {
        public long Count { get; set; }
        public IEnumerable<StoreDto> Stores { get; set; }
    }
}