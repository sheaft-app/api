using Sheaft.Interop.Enums;
using System;

namespace Sheaft.Models.Inputs
{
    public class CreateDocumentInput
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public DocumentKind Kind { get; set; }
    }
}