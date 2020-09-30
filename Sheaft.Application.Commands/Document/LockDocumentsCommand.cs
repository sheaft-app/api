using System;
using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Commands
{
    public class LockDocumentsCommand : Command<bool>
    {
        [JsonConstructor]
        public LockDocumentsCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid LegalId { get; set; }
    }
}
