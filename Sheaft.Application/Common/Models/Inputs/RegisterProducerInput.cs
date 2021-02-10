using System.Collections.Generic;

namespace Sheaft.Application.Models
{
    public class RegisterProducerInput : BusinessInput
    {
        public CreateBusinessLegalInput Legals { get; set; }
        public string SponsoringCode { get; set; }
    }
}