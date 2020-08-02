using System;

namespace Sheaft.Models.Inputs
{
    public class UpdatePackagingInput : CreatePackagingInput
    {
        public Guid Id { get; set; }
    }
}