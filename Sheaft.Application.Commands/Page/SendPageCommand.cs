using System;
using Sheaft.Core;
using Newtonsoft.Json;
using System.IO;

namespace Sheaft.Application.Commands
{
    public class SendPageCommand : Command<bool>
    {
        [JsonConstructor]
        public SendPageCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid DocumentId { get; set; }
        public Guid PageId { get; set; }
    }
}
