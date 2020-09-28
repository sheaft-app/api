﻿using System;
using Sheaft.Core;
using Newtonsoft.Json;
using Sheaft.Domain.Enums;

namespace Sheaft.Application.Commands
{
    public class CreateDocumentCommand : Command<Guid>
    {
        [JsonConstructor]
        public CreateDocumentCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid LegalId { get; set; }
        public string Name { get; set; }
        public DocumentKind Kind { get; set; }
    }
}
