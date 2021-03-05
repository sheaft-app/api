namespace Sheaft.Application.Common.Models.Dto
{
    public class BankAccountDto
    {
        public string IBAN { get; set; }
        public string BIC { get; set; }
        public string Owner { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Zipcode { get; set; }
        public string City { get; set; }
    }
}