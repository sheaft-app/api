using System;
using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Commands
{
    public class PayOrderCommand : Command<Guid>
    {
        [JsonConstructor]
        public PayOrderCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid OrderId { get; set; }
        public decimal Donation { get; set; }
    }
}
