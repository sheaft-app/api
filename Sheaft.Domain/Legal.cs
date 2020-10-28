using Sheaft.Domain.Interop;
using Sheaft.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using Sheaft.Exceptions;

namespace Sheaft.Domain.Models
{
    public abstract class Legal : IEntity
    {
        private List<Document> _documents;

        protected Legal()
        {
        }

        protected Legal(Guid id, LegalKind kind, User user, Owner owner)
        {
            Id = id;
            Kind = kind;
            Owner = owner;
            User = user;
        }

        public Guid Id { get; private set; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset? UpdatedOn { get; private set; }
        public DateTimeOffset? RemovedOn { get; private set; }
        public LegalKind Kind { get; protected set; }
        public virtual User User { get; private set; }
        public virtual Owner Owner { get; private set; }
        public virtual IReadOnlyCollection<Document> Documents => _documents?.AsReadOnly();

        public Document AddDocument(DocumentKind kind, string name)
        {
            if (Documents == null)
                _documents = new List<Document>();

            var document = new Document(Guid.NewGuid(), kind, name);
            _documents.Add(document);

            return document;
        }

        public void DeleteDocument(Guid id)
        {
            if (Documents == null)
                throw new NotFoundException(id);

            var document = _documents.FirstOrDefault(d => d.Id == id);
            if (document == null)
                throw new NotFoundException(id);

            _documents.Remove(document);
        }
    }
}