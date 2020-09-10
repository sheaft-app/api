using Sheaft.Interop;
using Sheaft.Interop.Enums;
using System;

namespace Sheaft.Domain.Models
{
    public class Document : IEntity
    {
        protected Document()
        {
        }

        public Document(Guid id, DocumentKind kind, string identifier, string name)
        {
            Id = id;
            Name = name;
            Kind = kind;
            Status = ValidationStatus.Created;
        }

        public Guid Id { get; private set; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset? UpdatedOn { get; private set; }
        public DateTimeOffset? RemovedOn { get; private set; }
        public DateTimeOffset? ProcessedDate { get; private set; }
        public string Identifier { get; private set; }
        public string Name { get; private set; }
        public string ResultCode { get; private set; }
        public string ResultMessage { get; private set; }
        public ValidationStatus Status { get; private set; }
        public DocumentKind Kind { get; private set; }
    }
}