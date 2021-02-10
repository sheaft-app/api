using System;

namespace Sheaft.Application.Common.Models.Inputs
{
    public class ConsumerInput
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Picture { get; set; }
    }
}