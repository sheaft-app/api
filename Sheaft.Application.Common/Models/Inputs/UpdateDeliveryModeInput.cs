using System;

namespace Sheaft.Application.Common.Models.Inputs
{
    public class UpdateDeliveryModeInput : CreateDeliveryModeInput
    {
        public Guid Id { get; set; }
    }
}