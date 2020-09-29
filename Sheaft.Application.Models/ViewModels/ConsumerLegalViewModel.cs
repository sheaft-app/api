using System;

namespace Sheaft.Application.Models
{
    public class ConsumerLegalViewModel
    {
        public Guid Id { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public DateTimeOffset? RemovedOn { get; set; }
        public OwnerViewModel Owner { get; set; }
    }
}
