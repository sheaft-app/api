using System.Collections.Generic;

namespace Sheaft.Models.Inputs
{
    public class RegisterProducerInput : CompanyInput
    {
        public RegisterOwnerInput Owner { get; set; }
    }
}