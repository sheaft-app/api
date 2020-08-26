using Sheaft.Interop.Enums;
using System;
using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Commands
{
    public class RestoreLevelCommand : Command<bool>
    {
        [JsonConstructor]
        public RestoreLevelCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
    }
}
