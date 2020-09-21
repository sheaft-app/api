using System;

namespace Sheaft.Application.Models
{
    public class RegisterConsumerInput : ConsumerInput
    {
        public Guid Id { get; set; }
        public string SponsoringCode { get; set; }
    }
}