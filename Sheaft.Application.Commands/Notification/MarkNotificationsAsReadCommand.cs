using System;
using Newtonsoft.Json;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class MarkUserNotificationsAsReadCommand : Command<bool>
    {
        [JsonConstructor]
        public MarkUserNotificationsAsReadCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public DateTimeOffset ReadBefore { get; set; }
    }
}