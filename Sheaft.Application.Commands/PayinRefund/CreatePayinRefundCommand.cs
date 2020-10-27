using Sheaft.Core;
using Newtonsoft.Json;
using System;

namespace Sheaft.Application.Commands
{
    public class CreatePayinRefundCommand : Command<Guid>
    {
        [JsonConstructor]
        public CreatePayinRefundCommand(RequestUser requestUser)
            : base(requestUser)
        {
        }

        public Guid PayinId { get; set; }
    }
}
