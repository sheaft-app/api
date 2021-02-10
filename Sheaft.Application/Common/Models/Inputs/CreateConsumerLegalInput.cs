using System;

namespace Sheaft.Application.Common.Models.Inputs
{
    public class CreateConsumerLegalInput : ConsumerLegalInput
    {
        public Guid UserId { get; set; }
    }
}