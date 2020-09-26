using Sheaft.Exceptions;
using Sheaft.Domain.Interop;
using Sheaft.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sheaft.Domain.Models
{
    public class UboDeclaration : IEntity
    {
        private List<Ubo> _ubos;

        protected UboDeclaration()
        {
        }

        public UboDeclaration(Guid id, BusinessLegal legals)
        {
            Id = id;
            Status = DeclarationStatus.WaitingForReview;
            Legal = legals;
        }

        public Guid Id { get; private set; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset? UpdatedOn { get; private set; }
        public DateTimeOffset? RemovedOn { get; private set; }
        public DateTimeOffset? ExecutedOn { get; private set; }
        public string Identifier { get; private set; }
        public DeclarationStatus Status { get; private set; }
        public string ReasonCode { get; private set; }
        public string ReasonMessage { get; private set; }
        public virtual BusinessLegal Legal { get; private set; }
        public virtual IReadOnlyCollection<Ubo> Ubos => _ubos?.AsReadOnly();

        public void AddUbo(Ubo ubo)
        {
            if (Ubos == null)
                _ubos = new List<Ubo>();

            var existingUbo = _ubos.FirstOrDefault(u => u.Id == ubo.Id);
            if (existingUbo == null)
                throw new ConflictException();

            _ubos.Add(ubo);
        }

        public void RemoveUbo(Ubo ubo)
        {
            if (Ubos == null)
                throw new NotFoundException();

            var existingUbo = _ubos.FirstOrDefault(u => u.Id == ubo.Id);
            if(existingUbo == null)
                throw new NotFoundException();

            _ubos.Remove(existingUbo);
        }


        public void SetStatus(DeclarationStatus status)
        {
            Status = status;
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
            if (ExecutedOn.HasValue)
                return;

            ExecutedOn = processedOn;
        }
    }
}