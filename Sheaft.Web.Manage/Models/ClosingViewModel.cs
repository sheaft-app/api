using System;

namespace Sheaft.Web.Manage.Models
{
    public class ClosingViewModel
    {
        public Guid? Id { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public DateTimeOffset? ClosedFrom { get; set; }
        public DateTimeOffset? ClosedTo { get; set; }
        public string Reason { get; set; }
        public bool Remove { get; set; }
    }
}