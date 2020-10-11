using System;

namespace Sheaft.Application.Models.Mailer
{
    public class AgreementMailerModel
    {
        public string Name { get; set; }
        public Guid AgreementId { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public string PortalUrl { get; set; }
    }
}
