using System;
using System.Collections.Generic;

namespace Sheaft.Web.Manage.Models
{
    public class ProductViewModel : ShortProductViewModel
    {
        public List<Guid> Tags { get; set; }
        public List<CatalogPriceViewModel> CatalogsPrices { get; set; }
        public List<PictureViewModel> Pictures { get; set; }
    }
}
