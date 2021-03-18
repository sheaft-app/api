using System;
using System.Collections.Generic;
using Sheaft.Domain.Enum;

namespace Sheaft.Web.Manage.Models
{
    public class LegalViewModel
    {
        public Guid Id { get; set; }
        public LegalKind Kind { get; set; }
        public LegalValidation Validation { get; set; }
        public OwnerViewModel Owner { get; set; }
        public IEnumerable<DocumentViewModel> Documents { get; set; }
    }
}
