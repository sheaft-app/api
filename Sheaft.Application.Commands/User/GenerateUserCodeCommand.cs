using System;
using Newtonsoft.Json;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class GenerateUserCodeCommand : Command<string>
    {
        [JsonConstructor]
        public GenerateUserCodeCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
    }
}
