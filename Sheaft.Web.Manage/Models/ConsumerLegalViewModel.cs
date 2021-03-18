using System;

namespace Sheaft.Web.Manage.Models
{
    public class ConsumerLegalViewModel : LegalViewModel
    {
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public DateTimeOffset? RemovedOn { get; set; }
    }
}
