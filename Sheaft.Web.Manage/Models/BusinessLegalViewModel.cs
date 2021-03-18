using System;

namespace Sheaft.Web.Manage.Models
{
    public class BusinessLegalViewModel : LegalViewModel
    {
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public DateTimeOffset? RemovedOn { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Siret { get; set; }
        public string VatIdentifier { get; set; }
        public AddressViewModel Address { get; set; }
        public DeclarationViewModel Declaration { get; set; }
    }
}
