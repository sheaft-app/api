using System;

namespace Sheaft.Models.Inputs
{
    public class UpdateReturnableInput : CreateReturnableInput
    {
        public Guid Id { get; set; }
    }
}