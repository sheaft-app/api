using System;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain
{
    public class Tag : IEntity
    {
        protected Tag()
        {
        }

        public Tag(TagKind kind, string name, string description = null, string picture = null, string icon = null)
        {
            Name = name;
            Kind = kind;
            Description = description;
            Picture = picture;
            Icon = icon;
        }

        public Guid Id { get; } = Guid.NewGuid();
        public TagKind Kind { get; set; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset UpdatedOn { get; private set; }
        public bool Removed { get; private set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }
        public string Icon { get; set; }
        public bool IsEnabled { get; set; }
        
        public void Restore()
        {
            Removed = false;
        }
    }
}