using Sheaft.Interop.Enums;
using System;

namespace Sheaft.Models.Dto
{
    public class JobDto
    {
        public Guid Id { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public DateTimeOffset? RemovedOn { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
        public string File { get; set; }
        public JobKind Kind { get; set; }
        public ProcessStatus Status { get; set; }
        public DateTimeOffset? StartedOn { get; set; }
        public DateTimeOffset? CompletedOn { get; set; }
        public bool Archived { get; set; }
        public int? Retried { get; set; }
        public UserProfileDto User { get; set; }
    }
}