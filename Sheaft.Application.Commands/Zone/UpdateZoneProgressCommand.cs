﻿using Newtonsoft.Json;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class UpdateZoneProgressCommand : Command<bool>
    {
        [JsonConstructor]
        public UpdateZoneProgressCommand(RequestUser requestUser) : base(requestUser)
        {
        }
    }
}
