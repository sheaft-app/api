using System;
using Sheaft.Domain.Enum;

namespace Sheaft.Application.Models
{
    public class CreateDocumentDto
    {
        public Guid LegalId { get; set; }
        public string Name { get; set; }
        public DocumentKind Kind { get; set; }
    }
}