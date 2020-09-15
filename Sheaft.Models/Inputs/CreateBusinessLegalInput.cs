using Sheaft.Interop.Enums;
using System;
using System.Collections.Generic;

namespace Sheaft.Models.Inputs
{
    public class CreateBusinessLegalInput : BusinessLegalInput
    {
        public Guid UserId { get; set; }
    }
}