using System;

namespace Sheaft.Application.Models
{
    public class CreatePreAuthorizationInput
    {
        public Guid UserId { get; set; }
        public Guid OrderId { get; set; }
        public Guid CardId { get; set; }
    }
}