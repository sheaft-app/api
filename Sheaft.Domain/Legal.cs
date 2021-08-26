using System;
using System.Collections.Generic;
using System.Linq;
using Sheaft.Core.Exceptions;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain
{
    public abstract class Legal : IIdEntity, ITrackCreation, ITrackUpdate
    {
        protected Legal()
        {
        }

        protected Legal(Guid id, LegalKind kind, User user, Owner owner)
        {
            Id = id;
            Kind = kind;
            Owner = owner;
            User = user;
            UserId = user.Id;

            Documents = new List<Document>();
        }

        public Guid Id { get; private set; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset? UpdatedOn { get; private set; }
        public LegalKind Kind { get; protected set; }
        public LegalValidation Validation { get; protected set; }
        public Guid UserId { get; private set; }
        public Owner Owner { get; private set; }
        public virtual User User { get; private set; }
        public virtual ICollection<Document> Documents { get; private set; }
        public byte[] RowVersion { get; private set; }

        public Document AddDocument(DocumentKind kind, string name)
        {
            if (Documents == null)
                Documents = new List<Document>();

            var document = new Document(Guid.NewGuid(), kind, name);
            Documents.Add(document);

            return document;
        }

        public void DeleteDocument(Guid id)
        {
            var document = Documents.FirstOrDefault(d => d.Id == id);
            if (document == null)
                throw SheaftException.Validation("Impossible de supprimer le document, il est introuvable.");

            Documents.Remove(document);
        }

        public virtual void SetKind(LegalKind kind)
        {
            Kind = kind;
        }

        public void SetValidation(LegalValidation validation)
        {
            Validation = validation;
        }
    }
}