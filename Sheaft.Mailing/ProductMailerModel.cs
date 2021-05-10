namespace Sheaft.Mailing
{
    public class ProductMailerModel
    {
        public string Name { get; set; }
        public string Reference { get; set; }
        public decimal WholeSalePrice { get; set; }
        public decimal OnSalePrice { get; set; }
        public decimal VatPrice { get; set; }
        public int Quantity { get; set; }
    }
}