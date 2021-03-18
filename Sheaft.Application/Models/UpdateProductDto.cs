using System;

namespace Sheaft.Application.Models
{
    public class UpdateProductDto : CreateProductDto
    {
        public Guid Id { get; set; }
    }
}