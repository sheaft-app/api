using System.Collections.Generic;

namespace Sheaft.Application.Common.Models.Inputs
{
    public class CreateQuickOrderInput : QuickOrderInput
    {
        public IEnumerable<ProductQuantityInput> Products { get; set; }
    }
}