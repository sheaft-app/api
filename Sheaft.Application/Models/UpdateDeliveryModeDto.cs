using System;

namespace Sheaft.Application.Models
{
    public class UpdateDeliveryModeDto : CreateDeliveryModeDto
    {
        public Guid Id { get; set; }
    }
}