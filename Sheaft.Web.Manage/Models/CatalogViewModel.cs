using System;
using System.Collections.Generic;
using Sheaft.Application.Models;
using Sheaft.Domain.Enum;

namespace Sheaft.Web.Manage.Models
{
    public class CatalogViewModel : ShortCatalogViewModel
    {
        public List<ProductPriceViewModel> Products { get; set; }
    }
}