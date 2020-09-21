using System;
using Newtonsoft.Json;
using Sheaft.Exceptions;
using Sheaft.Domain.Interop;
using Sheaft.Domain.Enums;

namespace Sheaft.Domain.Models
{
    public class Job : IEntity
    {
        protected Job()
        {
        }

        public Job(Guid id, JobKind kind, string name, User user, string queue)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ValidationException(MessageKind.Job_Name_Required);

            if (string.IsNullOrWhiteSpace(queue))
                throw new ValidationException(MessageKind.Job_Queue_Required);

            if (user == null)
                throw new ValidationException(MessageKind.Job_User_Required);

            Id = id;
            Name = name;
            User = user;
            Queue = queue;
            Status = ProcessStatus.Waiting;
            Kind = kind;
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
        public virtual User User { get; private set; }

        public void StartJob()
        {
            if (StartedOn.HasValue || Status == ProcessStatus.Processing)
                return;

            StartedOn = DateTimeOffset.UtcNow;
            Status = ProcessStatus.Processing;
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

        public void RetryJob()
        {
            if (Status != ProcessStatus.Cancelled && Status != ProcessStatus.Failed)
                throw new BadRequestException(MessageKind.Job_CannotRetry_NotIn_CanncelledOrFailedStatus);

            StartedOn = null;
            Retried = Retried.HasValue ? Retried + 1 : 1;
            Status = ProcessStatus.Waiting;
        }

        public void PauseJob()
        {
            if (!StartedOn.HasValue || Status != ProcessStatus.Processing)
                throw new BadRequestException(MessageKind.Job_CannotPause_NotIn_ProcessingStatus);

            Status = ProcessStatus.Paused;
        }

        public void ArchiveJob()
        {
            if (Status != ProcessStatus.Done && Status != ProcessStatus.Failed && Status != ProcessStatus.Cancelled)
                throw new BadRequestException(MessageKind.Job_CannotArchive_NotIn_TerminatedStatus);

            Archived = true;
        }

        public void UnarchiveJob()
        {
            Archived = false;
        }

        public void ResumeJob()
        {
            if (!StartedOn.HasValue || Status != ProcessStatus.Paused)
                throw new BadRequestException(MessageKind.Job_CannotResume_NotIn_PausedStatus);

            Status = ProcessStatus.Processing;
        }

        public void CompleteJob()
        {
            if (!StartedOn.HasValue || Status != ProcessStatus.Processing)
                throw new BadRequestException(MessageKind.Job_CannotComplete_NotIn_ProcessingStatus);

            CompletedOn = DateTimeOffset.UtcNow;
            Status = ProcessStatus.Done;
        }

        public void CancelJob(string message)
        {
            if (Status == ProcessStatus.Done)
                throw new BadRequestException(MessageKind.Job_CannotCancel_AlreadyDone);

            if (Status == ProcessStatus.Cancelled)
                throw new BadRequestException(MessageKind.Job_CannotCancel_AlreadyCancelled);

            Status = ProcessStatus.Cancelled;
            Message = message;
        }

        public void FailJob(string message)
        {
            if (Status == ProcessStatus.Done)
                throw new BadRequestException(MessageKind.Job_CannotFail_AlreadyDone);

            if (Status == ProcessStatus.Cancelled)
                throw new BadRequestException(MessageKind.Job_CannotFail_AlreadyCancelled);

            Status = ProcessStatus.Failed;
            Message = message;
        }

        public void SetCommand<T>(T command)
        {
            if (command == null)
                throw new ValidationException(MessageKind.Job_Command_Required);

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
    }
}