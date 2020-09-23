using System.Collections.Generic;

namespace Sheaft.Application.Models
{
    public class RegisterProducerInput : BusinessInput
    {
        public BusinessLegalInput Legals { get; set; }
        public string SponsoringCode { get; set; }
    }
}