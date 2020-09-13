using Sheaft.Interop.Enums;
using System;

namespace Sheaft.Models.Inputs
{
    public class SetConsumerLegalsInput
    {
        public Guid Id { get; set; }
        public OwnerInput Owner { get; set; }
    }
}