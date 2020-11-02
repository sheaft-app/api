using Sheaft.Domain.Enums;
using System;

namespace Sheaft.Application.Models
{
    public class TagViewModel
    {
        public Guid Id { get; set; }
        public TagKind Kind { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public DateTimeOffset? RemovedOn { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }
        public string Icon { get; set; }
        public bool Available { get; set; }
    }
}
