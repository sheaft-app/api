using System;
using Sheaft.Domain.Enum;

namespace Sheaft.Application.Common.Models.Dto
{
    public class TagDto
    {
        public Guid Id { get; set; }
        public TagKind Kind { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public bool Available { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }
        public string Icon { get; set; }
    }
}