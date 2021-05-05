using System.Collections.Generic;
using Sheaft.Domain;

namespace Sheaft.Application.Models
{
    public class StoresSearchDto
    {
        public long Count { get; set; }
        public IEnumerable<Store> Stores { get; set; }
    }
}