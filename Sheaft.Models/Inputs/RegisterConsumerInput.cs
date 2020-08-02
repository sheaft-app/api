using System;

namespace Sheaft.Models.Inputs
{
    public class RegisterConsumerInput : UserInput
    {
        public Guid Id { get; set; }
        public string SponsoringCode { get; set; }
        public bool Anonymous { get; set; }
        public Guid DepartmentId { get; set; }
    }
}