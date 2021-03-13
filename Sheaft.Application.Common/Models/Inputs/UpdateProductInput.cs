using System;

namespace Sheaft.Application.Common.Models.Inputs
{
    public class UpdateProductInput : CreateProductInput
    {
        public Guid Id { get; set; }
    }
}