namespace Sheaft.Application.Common.Models.Inputs
{
    public class RegisterProducerInput : BusinessInput
    {
        public CreateBusinessLegalInput Legals { get; set; }
        public string SponsoringCode { get; set; }
    }
}