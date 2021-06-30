using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Sheaft.Core.Enums;
using Sheaft.Core.Exceptions;
using Sheaft.Domain.Common;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain
{
    public class Job : IEntity, IHasDomainEvent
    {
        protected Job()
        {
        }

        public Job(Guid id, JobKind kind, string name, User user, object command)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw SheaftException.Validation("Le nom de la tâche est requis.");

            if (user == null)
                throw SheaftException.Validation("L'utilisateur rattaché à la tâche est requis.");

            Id = id;
            Name = name;
            User = user;
            UserId = user.Id;
            Status = ProcessStatus.Waiting;
            Kind = kind;
            DomainEvents = new List<DomainEvent>();
            SetCommand(command);
        }

        public Guid Id { get; private set; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset? UpdatedOn { get; private set; }
        public DateTimeOffset? RemovedOn { get; private set; }
        public string Name { get; private set; }
        public string Message { get; private set; }
        public string Command { get; private set; }
        public string File { get; private set; }
        public string Queue { get; private set; }
        public JobKind Kind { get; private set; }
        public ProcessStatus Status { get; private set; }
        public DateTimeOffset? StartedOn { get; private set; }
        public DateTimeOffset? CompletedOn { get; private set; }
        public bool Archived { get; private set; }
        public int? Retried { get; private set; }
        public Guid UserId { get; private set; }
        public virtual User User { get; private set; }

        public void StartJob(DomainEvent @event = null)
        {
            if(StartedOn.HasValue)
                throw SheaftException.Validation("Cette tâche est déjà en cours d'execution.");

            if (Status != ProcessStatus.Waiting)
                throw SheaftException.Validation("Cette tâche n'est pas en attente.");

            StartedOn = DateTimeOffset.UtcNow;
            Status = ProcessStatus.Processing;
            
            if(@event != null)
                DomainEvents.Add(@event);
        }

        public void SetName(string name)
        {
            if (name == null)
                return;

            Name = name;
        }

        public void SetDownloadUrl(string url)
        {
            if (url == null)
                return;

            File = url;
        }

        public void RetryJob(DomainEvent @event = null)
        {
            if (Status != ProcessStatus.Cancelled && Status != ProcessStatus.Failed)
                throw SheaftException.Validation("Impossible de reinitialiser cette tâche, elle n'est pas annulée ou en erreur.");

            StartedOn = null;
            Retried = Retried.HasValue ? Retried + 1 : 1;
            Status = ProcessStatus.Waiting;
            
            if(@event != null)
                DomainEvents.Add(@event);
        }

        public void PauseJob(DomainEvent @event = null)
        {
            if (!StartedOn.HasValue || Status != ProcessStatus.Processing)
                throw SheaftException.Validation("Impossible de mettre en pause cette tâche, elle n'est pas en cours d'execution.");

            Status = ProcessStatus.Paused;
            
            if(@event != null)
                DomainEvents.Add(@event);
        }

        public void ArchiveJob(DomainEvent @event = null)
        {
            if (Status != ProcessStatus.Done && Status != ProcessStatus.Failed && Status != ProcessStatus.Cancelled)
                throw SheaftException.Validation(
                    "Impossible d'archiver cette tâche, elle n'est pas terminé, en erreur ou annulée.");

            Archived = true;
            
            if(@event != null)
                DomainEvents.Add(@event);
        }

        public void UnarchiveJob(DomainEvent @event = null)
        {
            Archived = false;
            
            if(@event != null)
                DomainEvents.Add(@event);
        }

        public void ResumeJob(DomainEvent @event = null)
        {
            if (!StartedOn.HasValue || Status != ProcessStatus.Paused)
                throw SheaftException.Validation("Impossible de reprendre l'execution de cette tâche, elle n'est pas en pause.");

            Status = ProcessStatus.Processing;
            
            if(@event != null)
                DomainEvents.Add(@event);
        }

        public void CompleteJob(DomainEvent @event = null)
        {
            if (!StartedOn.HasValue || Status != ProcessStatus.Processing)
                throw SheaftException.Validation("Impossible de terminer cette tâche, elle n'est pas en cours d'execution.");

            CompletedOn = DateTimeOffset.UtcNow;
            Status = ProcessStatus.Done;
            
            if(@event != null)
                DomainEvents.Add(@event);
        }

        public void CancelJob(string message, DomainEvent @event = null)
        {
            if (Status == ProcessStatus.Done)
                throw SheaftException.Validation("Impossible d'annuler cette tâche, elle est déjà terminée.");

            if (Status == ProcessStatus.Cancelled)
                throw SheaftException.Validation("Impossible d'annuler cette tâche, elle est déjà annulée.");

            Status = ProcessStatus.Cancelled;
            Message = message;
            
            if(@event != null)
                DomainEvents.Add(@event);
        }

        public void FailJob(string message, DomainEvent @event = null)
        {
            if (Status == ProcessStatus.Done)
                throw SheaftException.Validation("Impossible d'indiquer cette tâche en erreur, elle est déjà terminée.");

            if (Status == ProcessStatus.Cancelled)
                throw SheaftException.Validation("Impossible d'indiquer cette tâche en erreur, elle est déjà annulée.");

            Status = ProcessStatus.Failed;
            Message = message;
            
            if(@event != null)
                DomainEvents.Add(@event);
        }

        public void SetCommand<T>(T command) where T:class
        {
            if (command == null)
                throw SheaftException.Validation("La commande à executer par cette tâche est requise.");

            Command = JsonConvert.SerializeObject(command);
        }

        public T GetCommand<T>() where T:class
        {
            if (string.IsNullOrWhiteSpace(Command))
                return default;

            return JsonConvert.DeserializeObject<T>(Command);
        }

        public void ResetJob()
        {
            StartedOn = null;
            CompletedOn = null;
            Status = ProcessStatus.Waiting;
            Retried = null;
            Message = null;
        }

        public List<DomainEvent> DomainEvents { get; } = new List<DomainEvent>();
        public byte[] RowVersion { get; private set; }
    }
}