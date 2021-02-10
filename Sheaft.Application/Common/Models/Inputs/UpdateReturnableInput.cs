using System;

namespace Sheaft.Application.Common.Models.Inputs
{
    public class UpdateReturnableInput : CreateReturnableInput
    {
        public Guid Id { get; set; }
    }
}