using System;
using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Commands
{
    public class CheckPayinCommand : Command<bool>
    {
        public const string QUEUE_NAME = "command-check-payin";

        [JsonConstructor]
        public CheckPayinCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid PayinId { get; set; }
    }
}
