using Sheaft.Interop.Enums;
using System;

namespace Sheaft.Models.ViewModels
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
        public string Image { get; set; }
    }
}
