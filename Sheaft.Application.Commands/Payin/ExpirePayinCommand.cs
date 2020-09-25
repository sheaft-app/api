using System;
using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Commands
{
    public class ExpirePayinCommand : Command<bool>
    {
        public const string QUEUE_NAME = "command-expire-payin";

        [JsonConstructor]
        public ExpirePayinCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid PayinId { get; set; }
    }
}
