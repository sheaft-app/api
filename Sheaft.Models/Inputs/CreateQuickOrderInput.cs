using System.Collections.Generic;

namespace Sheaft.Models.Inputs
{
    public class CreateQuickOrderInput : QuickOrderInput
    {
        public IEnumerable<ProductQuantityInput> Products { get; set; }
    }
}