using Sheaft.Interop.Enums;
using System;

namespace Sheaft.Models.Inputs
{
    public class CreateConsumerLegalInput : ConsumerLegalInput
    {
        public Guid UserId { get; set; }
    }
}