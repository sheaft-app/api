using System;

namespace Sheaft.Models.Inputs
{
    public class RegisterConsumerInput : ConsumerInput
    {
        public Guid Id { get; set; }
        public string SponsoringCode { get; set; }
    }
}