using System;

namespace Sheaft.Application.Models
{
    public class CatalogDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public bool VisibleToConsumers { get; set; }
        public bool VisibleToStores { get; set; }
        public bool IsDefaultForStores { get; set; }
        public int ProductsCount { get; set; }
    }
}