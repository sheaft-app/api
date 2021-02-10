using System;

namespace Sheaft.Application.Models
{
    public class RateProductInput
    {
        public Guid Id { get; set; }
        public decimal Value { get; set; }
        public string Comment { get; set; }
    }
}