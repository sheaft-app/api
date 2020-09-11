namespace Sheaft.Models.Inputs
{
    public class CreateReturnableInput
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal WholeSalePrice { get; set; }
        public decimal Vat { get; set; }
    }
}