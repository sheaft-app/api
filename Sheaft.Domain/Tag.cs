using System;
using Sheaft.Exceptions;
using Sheaft.Interop;
using Sheaft.Interop.Enums;

namespace Sheaft.Domain.Models
{
    public class Tag : IEntity
    {
        protected Tag()
        {
        }

        public Tag(Guid id, TagKind kind, string name, string description = null, string image = null)
        {
            Id = id;

            SetKind(kind);
            SetName(name);
            SetImage(image);
            SetDescription(description);
        }

        public Guid Id { get; private set; }
        public TagKind Kind { get; private set; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset? UpdatedOn { get; private set; }
        public DateTimeOffset? RemovedOn { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string Image { get; private set; }

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ValidationException(MessageKind.Tag_Name_Required);

            Name = name;
        }

        public void SetKind(TagKind kind)
        {
            Kind = kind;
        }

        public void SetImage(string image)
        {
            if (image == null)
                return;

            Image = image;
        }

        public void SetDescription(string description)
        {
            Description = description;
        }

        public void Remove()
        {
        }

        public void Restore()
        {
            RemovedOn = null;
        }
    }
}