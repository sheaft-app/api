using System;
using Sheaft.Core.Exceptions;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain
{
    public class Tag : IEntity
    {
        protected Tag()
        {
        }

        public Tag(Guid id, TagKind kind, string name, string description = null, string picture = null)
        {
            Id = id;

            SetKind(kind);
            SetName(name);
            SetPicture(picture);
            SetDescription(description);
        }

        public Guid Id { get; private set; }
        public TagKind Kind { get; private set; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset? UpdatedOn { get; private set; }
        public DateTimeOffset? RemovedOn { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string Picture { get; private set; }
        public string Icon { get; private set; }
        public bool Available { get; private set; }
        public byte[] RowVersion { get; private set; }

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw SheaftException.Validation("Le nom est requis.");

            Name = name;
        }

        public void SetKind(TagKind kind)
        {
            Kind = kind;
        }

        public void SetAvailable(bool available)
        {
            Available = available;
        }

        public void SetPicture(string picture)
        {
            if (picture == null)
                return;

            Picture = picture;
        }

        public void SetIcon(string icon)
        {
            if (icon == null)
                return;

            Icon = icon;
        }

        public void SetDescription(string description)
        {
            Description = description;
        }
    }
}