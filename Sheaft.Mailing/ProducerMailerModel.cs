using System.Collections.Generic;

namespace Sheaft.Mailing
{
    public class ProducerMailerModel
    {
        public string Name { get; set; }
        public AddressMailerModel Address { get; set; }
        public ExpectedOrderDeliveryMailerModel Delivery { get; set; }
        public IEnumerable<ProductMailerModel> Products { get; set; }
        public decimal WholeSalePrice { get; set; }
        public decimal OnSalePrice { get; set; }
        public decimal VatPrice { get; set; }
    }
}