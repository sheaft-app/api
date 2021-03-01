using System;
using System.Collections.Generic;

namespace Sheaft.Application.Common.Models.Inputs
{
    public class CreateBusinessClosingsInput
    {
        public Guid UserId { get; set; }
        public List<ClosingInput> Closings { get; set; }
    }
}