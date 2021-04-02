using System;

namespace Sheaft.Application.Models
{
    public class CloneCatalogDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal? Percent { get; set; }
    }
}