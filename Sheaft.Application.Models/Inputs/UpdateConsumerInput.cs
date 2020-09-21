using Sheaft.Domain.Enums;
using System;

namespace Sheaft.Application.Models
{
    public class UpdateConsumerInput : ConsumerInput
    {
        public Guid Id { get; set; }
    }
}