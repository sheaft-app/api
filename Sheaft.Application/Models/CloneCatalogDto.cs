using System;

namespace Sheaft.Application.Models
{
    public class CloneCatalogDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool VisibleToConsumers { get; set; }
        public bool VisibleToStores { get; set; }
        public bool IsDefaultForStores { get; set; }
        public decimal Percent { get; set; }
    }
}