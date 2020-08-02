using System;

namespace Sheaft.Models.Inputs
{
    public class UpdateDeliveryModeInput : CreateDeliveryModeInput
    {
        public Guid Id { get; set; }
    }
}