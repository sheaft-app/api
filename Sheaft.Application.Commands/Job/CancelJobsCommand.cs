﻿using System;
using System.Collections.Generic;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class CancelJobsCommand : Command<bool>
    {
        public CancelJobsCommand(RequestUser user) : base(user)
        {
        }

        public IEnumerable<Guid> Ids { get; set; }
        public string Reason { get; set; }
    }
}
