using System;

namespace Sheaft.Web.Manage.Models
{
    public class ShortBatchViewModel
    {
        public Guid Id { get; set; }
        public string Number { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public DateTimeOffset? RemovedOn { get; set; }
        public DateTimeOffset? DLC { get; set; }
        public DateTimeOffset? DLUO { get; set; }
        public string Comment { get; set; }
        public Guid ProducerId { get; set; }
        public UserViewModel Producer { get; set; }
    }
}