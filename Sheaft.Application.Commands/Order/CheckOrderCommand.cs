using Sheaft.Core;
using Newtonsoft.Json;
using Sheaft.Domain.Enums;
using System;

namespace Sheaft.Application.Commands
{
    public class CheckOrderCommand : Command<bool>
    {
        public const string QUEUE_NAME = "command-check-order";

        [JsonConstructor]
        public CheckOrderCommand(RequestUser requestUser)
            : base(requestUser)
        {
        }
        public Guid OrderId { get; set; }
    }
}
