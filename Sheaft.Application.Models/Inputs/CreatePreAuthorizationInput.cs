using System;

namespace Sheaft.Application.Models
{
    public class CreatePreAuthorizationInput
    {
        public Guid OrderId { get; set; }
        public string CardIdentifier { get; set; }
    }
}