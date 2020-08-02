using Sheaft.Interop.Enums;
using System;

namespace Sheaft.Models.Dto
{
    public class TagDto
    {
        public Guid Id { get; set; }
        public TagKind Kind { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
    }
}