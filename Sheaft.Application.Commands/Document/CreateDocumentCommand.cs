using System;
using Sheaft.Core;
using Newtonsoft.Json;
using Sheaft.Interop.Enums;

namespace Sheaft.Application.Commands
{
    public class CreateDocumentCommand : Command<Guid>
    {
        [JsonConstructor]
        public CreateDocumentCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid UserId { get; set; }
        public string Name { get; set; }
        public DocumentKind Kind { get; set; }
    }
}
