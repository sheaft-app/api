using Sheaft.Interop.Enums;
using System;
using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Commands
{
    public class DeleteLevelCommand : Command<bool>
    {
        [JsonConstructor]
        public DeleteLevelCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
    }
}
