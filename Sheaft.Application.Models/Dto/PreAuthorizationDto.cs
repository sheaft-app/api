using System;
using Sheaft.Domain.Enums;

namespace Sheaft.Application.Models
{
    public class PreAuthorizationDto
    {
        public Guid Id { get; set; }
        public PreAuthorizationStatus Status { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public string SecureModeRedirectURL { get; set; }
    }
}