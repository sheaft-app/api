using System;
using System.Collections.Generic;
using Sheaft.Application.Models;
using Sheaft.Domain.Enum;

namespace Sheaft.Web.Manage.Models
{
    public class CatalogViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public CatalogKind Kind { get; set; }
        public bool Available { get; set; }
        public bool IsDefault { get; set; }
        public UserViewModel Producer { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public DateTimeOffset? RemovedOn { get; set; }
        public List<ProductPriceViewModel> Products { get; set; }
        public int ProductsCount { get; set; }
    }
}