using System;
using Sheaft.Core;
using Newtonsoft.Json;
using Sheaft.Domain.Enums;

namespace Sheaft.Application.Commands
{
    public class UpdateDocumentCommand : Command<bool>
    {
        [JsonConstructor]
        public UpdateDocumentCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public DocumentKind Kind { get; set; }
    }
}
