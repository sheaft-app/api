using System;
using System.Collections.Generic;
using System.Linq;
using Sheaft.Domain.Common;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Events.Document;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain
{
    public class Document : IIdEntity, ITrackCreation, ITrackUpdate, IHasDomainEvent
    {
        protected Document()
        {
        }

        public Document(Guid id, DocumentKind kind, string name)
        {
            Id = id;
            Name = name;
            Kind = kind;
            Status = DocumentStatus.UnLocked;

            Pages = new List<Page>();
            DomainEvents = new List<DomainEvent>();
        }

        public Guid Id { get; private set; }
        public DocumentKind Kind { get; private set; }
        public DocumentStatus Status { get; private set; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset? UpdatedOn { get; private set; }
        public DateTimeOffset? ProcessedOn { get; private set; }
        public string Identifier { get; private set; }
        public string Name { get; private set; }
        public string ResultCode { get; private set; }
        public string ResultMessage { get; private set; }
        public Guid LegalId { get; private set; }
        public int PagesCount { get; private set; }
        public virtual ICollection<Page> Pages { get; private set; }

        public void SetIdentifier(string identifier)
        {
            if (identifier == null)
                return;

            Identifier = identifier;
        }

        public void SetStatus(DocumentStatus status)
        {
            Status = status;
            
            switch (Status)
            {
                case DocumentStatus.Refused:
                    DomainEvents.Add(new DocumentRefusedEvent(Id));
                    break;
                case DocumentStatus.OutOfDate:
                    DomainEvents.Add(new DocumentOutdatedEvent(Id));
                    break;
            }
        }

        public void SetResult(string code, string message)
        {
            ResultCode = code;
            ResultMessage = message;
        }

        public void AddPage(Page page)
        {
            if (Pages == null)
                Pages = new List<Page>();

            Pages.Add(page);
            PagesCount = Pages?.Count ?? 0;
        }

        public void SetProcessedOn(DateTimeOffset? processedOn)
        {
            if (ProcessedOn.HasValue)
                return;

            ProcessedOn = processedOn;
        }

        public void SetKind(DocumentKind kind)
        {
            Kind = kind;
        }

        public void SetName(string name)
        {
            if (name == null)
                return;

            Name = name;
        }

        public void DeletePage(Guid pageId)
        {
            if (Pages == null)
                return;

            var page = Pages.FirstOrDefault(p => p.Id == pageId);
            Pages.Remove(page);
            PagesCount = Pages?.Count ?? 0;
        }

        public List<DomainEvent> DomainEvents { get; } = new List<DomainEvent>();
        public byte[] RowVersion { get; private set; }
    }
}