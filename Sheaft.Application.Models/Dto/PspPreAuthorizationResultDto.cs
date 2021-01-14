using Sheaft.Domain.Enums;
using System;

namespace Sheaft.Application.Models
{
    public class PspPreAuthorizationResultDto: PspResultDto
    {
        public PreAuthorizationStatus Status { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public string ResultCode { get; set; }
        public string ResultMessage { get; set; }
        public decimal Remaining { get; set; }
        public decimal Debited { get; set; }
        public DateTimeOffset? ExpirationDate { get; set; }
    }
}