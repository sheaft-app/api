using System;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class RateProductCommand : Command<bool>
    {
        public RateProductCommand(RequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
        public decimal Value { get; set; }
        public string Comment { get; set; }
    }
}
