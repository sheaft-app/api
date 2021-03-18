using System;

namespace Sheaft.Application.Models
{
    public class UpdateConsumerLegalDto : CreateConsumerLegalDto
    {
        public Guid Id { get; set; }
    }
}