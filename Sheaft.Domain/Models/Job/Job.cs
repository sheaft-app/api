using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Sheaft.Domain.Common;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Events;
using Sheaft.Domain.Exceptions;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain
{
    public class Job : IEntity, IHasDomainEvent
    {
        protected Job()
        {
        }

        public Job(JobKind kind, string name, User user, object command)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ValidationException("Le nom de la tâche est requis.");

            if (user == null)
                throw new ValidationException("L'utilisateur rattaché à la tâche est requis.");

            Name = name;
            User = user;
            UserId = user.Id;
            Status = ProcessStatus.Pending;
            Kind = kind;
            SetCommand(command);
        }

        public Guid Id { get; } = Guid.NewGuid();
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset UpdatedOn { get; private set; }
        public bool Removed { get; private set; }
        public string Name { get; set; }
        public string Message { get; private set; }
        public string Command { get; private set; }
        public string ResultFileUrl { get; private set; }
        public JobKind Kind { get; private set; }
        public ProcessStatus Status { get; private set; }
        public DateTimeOffset? StartedOn { get; private set; }
        public DateTimeOffset? CompletedOn { get; private set; }
        public bool IsArchived { get; set; }
        public Guid UserId { get; private set; }
        public User User { get; private set; }
        public List<DomainEvent> DomainEvents { get; private set; } = new List<DomainEvent>();
        
        public void Restore()
        {
            Removed = false;
        }

        public void StartJob(DomainEvent @event = null)
        {
            if(StartedOn.HasValue)
                throw new ValidationException("Cette tâche est déjà en cours d'execution.");

            if (Status != ProcessStatus.Pending)
                throw new ValidationException("Cette tâche n'est pas en attente.");

            StartedOn = DateTimeOffset.UtcNow;
            Status = ProcessStatus.Processing;
            
            if(@event != null)
                DomainEvents.Add(@event);
        }

        public void CompleteJob(DomainEvent @event = null)
        {
            if (!StartedOn.HasValue || Status != ProcessStatus.Processing)
                throw new ValidationException("Impossible de terminer cette tâche, elle n'est pas en cours d'execution.");

            CompletedOn = DateTimeOffset.UtcNow;
            Status = ProcessStatus.Done;
            
            if(@event != null)
                DomainEvents.Add(@event);
        }

        public void CancelJob(string message, DomainEvent @event = null)
        {
            if (Status == ProcessStatus.Done)
                throw new ValidationException("Impossible d'annuler cette tâche, elle est déjà terminée.");

            if (Status == ProcessStatus.Cancelled)
                throw new ValidationException("Impossible d'annuler cette tâche, elle est déjà annulée.");

            Status = ProcessStatus.Cancelled;
            Message = message;
            
            if(@event != null)
                DomainEvents.Add(@event);
        }

        public void FailJob(string message, DomainEvent @event = null)
        {
            if (Status == ProcessStatus.Done)
                throw new ValidationException("Impossible d'indiquer cette tâche en erreur, elle est déjà terminée.");

            if (Status == ProcessStatus.Cancelled)
                throw new ValidationException("Impossible d'indiquer cette tâche en erreur, elle est déjà annulée.");

            Status = ProcessStatus.Failed;
            Message = message;
            
            if(@event != null)
                DomainEvents.Add(@event);
        }

        public void SetCommand<T>(T command) where T:class
        {
            if (command == null)
                throw new ValidationException("La commande à executer par cette tâche est requise.");

            Command = JsonConvert.SerializeObject(command);
        }

        public T GetCommand<T>() where T:class
        {
            if (string.IsNullOrWhiteSpace(Command))
                return default;

            return JsonConvert.DeserializeObject<T>(Command);
        }
    }
}