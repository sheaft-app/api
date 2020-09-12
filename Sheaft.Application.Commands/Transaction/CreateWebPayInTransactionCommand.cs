using System;
using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Commands
{
    public class CreateWebPayInTransactionCommand : Command<Guid>
    {
        [JsonConstructor]
        public CreateWebPayInTransactionCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid OrderId { get; set; }
    }
}
