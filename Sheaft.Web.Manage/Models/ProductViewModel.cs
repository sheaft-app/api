using System;
using System.Collections.Generic;
using Sheaft.Domain.Enum;

namespace Sheaft.Web.Manage.Models
{
    public class ProductViewModel : ShortProductViewModel
    {
        public List<Guid> Tags { get; set; }
        public List<CatalogPriceViewModel> CatalogsPrices { get; set; }
        public List<PictureViewModel> Pictures { get; set; }
    }
}
