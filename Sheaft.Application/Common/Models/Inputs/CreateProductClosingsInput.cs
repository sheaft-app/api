using System;
using System.Collections.Generic;

namespace Sheaft.Application.Common.Models.Inputs
{
    public class CreateProductClosingsInput
    {
        public Guid ProductId { get; set; }
        public List<ClosingInput> Closings { get; set; }
    }
}