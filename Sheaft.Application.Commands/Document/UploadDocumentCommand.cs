using System;
using Sheaft.Core;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Sheaft.Application.Commands
{
    public class UploadDocumentCommand : Command<bool>
    {
        [JsonConstructor]
        public UploadDocumentCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid DocumentId { get; set; }
        public IList<UploadPageCommand> Pages { get; set; }
    }
}
