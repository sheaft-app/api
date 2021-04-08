using Sheaft.Domain.Enum;

namespace Sheaft.Application.Models
{
    public class CreateCatalogDto
    {
        public string Name { get; set; }
        public bool IsDefault { get; set; }
        public bool IsAvailable { get; set; }
    }
}