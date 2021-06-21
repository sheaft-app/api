using System;
using Sheaft.Domain.Enum;

namespace Sheaft.Application.Models
{
    public class ReturnedProductDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public ModificationKind Kind { get; set; }
    }
}