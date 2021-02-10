using System;

namespace Sheaft.Application.Models
{
    public class CreateConsumerLegalInput : ConsumerLegalInput
    {
        public Guid UserId { get; set; }
    }
}