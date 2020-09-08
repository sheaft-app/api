using System;
using System.Collections.Generic;

namespace Sheaft.Models.Inputs
{
    public class RegisterOwnerInput : ConsumerInput
    {
        public Guid Id { get; set; }
        public string SponsoringCode { get; set; }
    }
}