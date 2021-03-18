namespace Sheaft.Application.Mailings
{
    public class PurchaseOrderLineMailerModel
    {
        public string Line_Name { get; set; }
        public int Line_Quantity { get; set; }
        public decimal Line_Price { get; set; }
    }
}
