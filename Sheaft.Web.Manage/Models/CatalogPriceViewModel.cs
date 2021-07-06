using System;

namespace Sheaft.Web.Manage.Models
{
    public class CatalogPriceViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal WholeSalePricePerUnit { get; set; }
        public bool Remove { get; set; }
    }
}