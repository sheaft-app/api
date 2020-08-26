using System;
using Newtonsoft.Json;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class RemoveUserDataCommand : Command<string>
    {
        public const string QUEUE_NAME = "command-users-removedata";

        [JsonConstructor]
        public RemoveUserDataCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
        public string Email { get; set; }
    }
}
