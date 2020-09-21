using System;

namespace Sheaft.Application.Models
{
    public class UpdateProductInput : CreateProductInput
    {
        public Guid Id { get; set; }
    }
}