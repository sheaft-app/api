using Sheaft.Domain.Enums;
using System;

namespace Sheaft.Application.Models
{
    public class CreateDocumentInput
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public DocumentKind Kind { get; set; }
    }
}