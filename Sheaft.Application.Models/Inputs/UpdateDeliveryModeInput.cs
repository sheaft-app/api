using System;

namespace Sheaft.Application.Models
{
    public class UpdateDeliveryModeInput : CreateDeliveryModeInput
    {
        public Guid Id { get; set; }
    }
}