using System;

namespace Sheaft.Models.Inputs
{
    public class UpdateProductInput : CreateProductInput
    {
        public Guid Id { get; set; }
    }
}