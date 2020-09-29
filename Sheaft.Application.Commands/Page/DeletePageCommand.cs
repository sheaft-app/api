using System;
using Sheaft.Core;
using Newtonsoft.Json;
using System.IO;

namespace Sheaft.Application.Commands
{
    public class DeletePageCommand : Command<bool>
    {
        [JsonConstructor]
        public DeletePageCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid DocumentId { get; set; }
        public Guid PageId { get; set; }
    }
}
