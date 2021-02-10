﻿using System;
using System.Collections.Generic;
using System.Linq;
using Sheaft.Domain.Common;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Events.Declaration;
using Sheaft.Domain.Exceptions;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain
{
    public class Declaration : IIdEntity, ITrackCreation, ITrackUpdate, IHasDomainEvent
    {
        private List<Ubo> _ubos;

        protected Declaration()
        {
        }

        public Declaration(Guid id)
        {
            Id = id;
            Status = DeclarationStatus.UnLocked;
            DomainEvents = new List<DomainEvent>();
        }

        public Guid Id { get; private set; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset? UpdatedOn { get; private set; }
        public DateTimeOffset? ProcessedOn { get; private set; }
        public string Identifier { get; private set; }
        public DeclarationStatus Status { get; private set; }
        public string ReasonCode { get; private set; }
        public string ReasonMessage { get; private set; }
        public virtual IReadOnlyCollection<Ubo> Ubos => _ubos?.AsReadOnly();

        public void AddUbo(Ubo ubo)
        {
            if (Ubos == null)
                _ubos = new List<Ubo>();

            var existingUbo = _ubos.FirstOrDefault(u => u.Id == ubo.Id);
            if (existingUbo != null)
                throw new ValidationException(MessageKind.Ubo_CannotAdd_AlreadyExists);

            _ubos.Add(ubo);
        }

        public void RemoveUbo(Guid id)
        {
            var existingUbo = _ubos.FirstOrDefault(u => u.Id == id);
            if(existingUbo == null)
                throw new ValidationException(MessageKind.Ubo_CannotRemove_NotFound);

            _ubos.Remove(existingUbo);
        }

        public void SetStatus(DeclarationStatus status)
        {
            Status = status;
            
            switch (Status)
            {
                case DeclarationStatus.Incomplete:
                    DomainEvents.Add(new DeclarationIncompleteEvent(Id));
                    break;
                case DeclarationStatus.Refused:
                    DomainEvents.Add(new DeclarationRefusedEvent(Id));
                    break;
                case DeclarationStatus.Validated:
                    DomainEvents.Add(new DeclarationValidatedEvent(Id));
                    break;
            }
        }

        public void SetIdentifier(string identifier)
        {
            Identifier = identifier;
        }

        public void SetResult(string code, string message)
        {
            ReasonCode = code;
            ReasonMessage = message;
        }

        public void SetProcessedOn(DateTimeOffset? processedOn)
        {
            if (ProcessedOn.HasValue)
                return;

            ProcessedOn = processedOn;
        }

        public List<DomainEvent> DomainEvents { get; } = new List<DomainEvent>();
    }
}