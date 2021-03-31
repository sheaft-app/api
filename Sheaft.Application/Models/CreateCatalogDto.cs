namespace Sheaft.Application.Models
{
    public class CreateCatalogDto
    {
        public string Name { get; set; }
        public bool VisibleToConsumers { get; set; }
        public bool VisibleToStores { get; set; }
        public bool IsDefaultForStores { get; set; }
    }
}