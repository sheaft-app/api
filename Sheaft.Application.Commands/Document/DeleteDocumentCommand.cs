using System;
using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Commands
{
    public class DeleteDocumentCommand : Command<bool>
    {
        [JsonConstructor]
        public DeleteDocumentCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
    }
}
