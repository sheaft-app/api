using System;
using Sheaft.Domain.Enum;

namespace Sheaft.Application.Common.Models.Inputs
{
    public class CreateDocumentInput
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public DocumentKind Kind { get; set; }
    }
}