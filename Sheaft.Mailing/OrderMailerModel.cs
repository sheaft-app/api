using System;

namespace Sheaft.Mailing
{
    public class OrderMailerModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public int ProductsCount { get; set; }
        public string MyOrdersUrl { get; set; }
    }
}
