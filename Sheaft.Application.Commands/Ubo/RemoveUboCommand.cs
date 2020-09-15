using System;
using Sheaft.Interop.Enums;
using Sheaft.Models.Inputs;
using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Commands
{

    public class RemoveUboCommand : Command<bool>
    {
        [JsonConstructor]
        public RemoveUboCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
    }
}
