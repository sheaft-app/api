using System;

namespace Sheaft.Models.Inputs
{
    public class RegisterConsumerInput : ConsumerInput
    {
        public Guid Id { get; set; }
        public string SponsoringCode { get; set; }
        public bool Anonymous { get; set; }
        public Guid DepartmentId { get; set; }
    }
}