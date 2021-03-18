namespace Sheaft.Application.Models
{
    public class RegisterProducerDto : BusinessDto
    {
        public CreateBusinessLegalDto Legals { get; set; }
        public string SponsoringCode { get; set; }
    }
}