using System.Collections.Generic;

namespace Sheaft.Web.Manage.Models
{
    public class CatalogViewModel : ShortCatalogViewModel
    {
        public List<ProductPriceViewModel> Products { get; set; }
    }
}