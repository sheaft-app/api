using System;

namespace Sheaft.Application.Models
{
    public class AssignCatalogToAgreementDto
    {
        public Guid CatalogId { get; set; }
        public Guid AgreementId { get; set; }
    }
}