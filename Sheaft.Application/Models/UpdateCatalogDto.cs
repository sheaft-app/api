using System;

namespace Sheaft.Application.Models
{
    public class UpdateCatalogDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsDefault { get; set; }
        public bool IsAvailable { get; set; }
    }
}