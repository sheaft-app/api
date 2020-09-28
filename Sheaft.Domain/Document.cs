using Sheaft.Domain.Interop;
using Sheaft.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Sheaft.Domain.Models
{
    public class Document : IEntity
    {
        private List<Page> _pages;

        protected Document()
        {
        }

        public Document(Guid id, DocumentKind kind, string name, User user)
        {
            Id = id;
            Name = name;
            Kind = kind;
            Status = DocumentStatus.WaitingForReview;
            User = user;

            _pages = new List<Page>();
        }

        public Guid Id { get; private set; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset? UpdatedOn { get; private set; }
        public DateTimeOffset? RemovedOn { get; private set; }
        public DateTimeOffset? ProcessedOn { get; private set; }
        public string Identifier { get; private set; }
        public string Name { get; private set; }
        public string ResultCode { get; private set; }
        public string ResultMessage { get; private set; }
        public DocumentStatus Status { get; private set; }
        public DocumentKind Kind { get; private set; }
        public virtual User User { get; private set; }
        public virtual IReadOnlyCollection<Page> Pages => _pages?.AsReadOnly();

        public void SetIdentifier(string identifier)
        {
            if (identifier == null)
                return;

            Identifier = identifier;
        }

        public void SetValidationStatus(DocumentStatus status)
        {
            Status = status;
        }

        public void SetResult(string code, string message)
        {
            ResultCode = code;
            ResultMessage = message;
        }

        public void AddPage(Page page)
        {
            if (Pages == null)
                _pages = new List<Page>();

            _pages.Add(page);
        }

        public void SetProcessedOn(DateTimeOffset? processedOn)
        {
            if (ProcessedOn.HasValue)
                return;

            ProcessedOn = processedOn;
        }
    }
}