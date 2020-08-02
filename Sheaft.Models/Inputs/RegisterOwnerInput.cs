using System;
using System.Collections.Generic;

namespace Sheaft.Models.Inputs
{
    public class RegisterOwnerInput : UserInput
    {
        public Guid Id { get; set; }
        public string SponsoringCode { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}