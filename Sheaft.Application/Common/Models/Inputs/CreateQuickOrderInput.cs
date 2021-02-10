using System.Collections.Generic;

namespace Sheaft.Application.Models
{
    public class CreateQuickOrderInput : QuickOrderInput
    {
        public IEnumerable<ProductQuantityInput> Products { get; set; }
    }
}