﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class SetProductsSearchabilityCommand : Command<bool>
    {
        [JsonConstructor]
        public SetProductsSearchabilityCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public IEnumerable<Guid> Ids { get; set; }
        public bool Searchable { get; set; }
    }
}