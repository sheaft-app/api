using System;

namespace Sheaft.Application.Models
{
    public class CreatePreAuthorizationDto
    {
        public Guid OrderId { get; set; }
        public string CardIdentifier { get; set; }
    }
}