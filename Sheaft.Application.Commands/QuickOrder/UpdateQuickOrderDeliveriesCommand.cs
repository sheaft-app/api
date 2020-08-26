using System;
using Newtonsoft.Json;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class UpdateQuickOrderDeliveriesCommand : Command<bool>
    {
        [JsonConstructor]
        public UpdateQuickOrderDeliveriesCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
    }
}
