using System;

namespace Sheaft.Application.Common.Models.Inputs
{
    public class RateProductInput
    {
        public Guid Id { get; set; }
        public decimal Value { get; set; }
        public string Comment { get; set; }
    }
}