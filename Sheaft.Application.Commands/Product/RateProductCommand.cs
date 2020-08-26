using System;
using Newtonsoft.Json;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class RateProductCommand : Command<bool>
    {
        [JsonConstructor]
        public RateProductCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
        public decimal Value { get; set; }
        public string Comment { get; set; }
    }
}
