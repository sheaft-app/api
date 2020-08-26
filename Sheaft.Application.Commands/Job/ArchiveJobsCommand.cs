using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class ArchiveJobsCommand : Command<bool>
    {
        [JsonConstructor]
        public ArchiveJobsCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public IEnumerable<Guid> Ids { get; set; }
    }
}
