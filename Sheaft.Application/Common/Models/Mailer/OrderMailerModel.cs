using System;

namespace Sheaft.Application.Models.Mailer
{
    public class OrderMailerModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public int ProductsCount { get; set; }
        public string Reference { get; set; }
        public Guid OrderId { get; set; }
        public string MyOrdersUrl { get; set; }
    }
}
