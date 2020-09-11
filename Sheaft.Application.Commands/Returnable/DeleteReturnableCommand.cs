using System;
using Newtonsoft.Json;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class DeleteReturnableCommand : Command<bool>
    {
        [JsonConstructor]
        public DeleteReturnableCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
    }
}
