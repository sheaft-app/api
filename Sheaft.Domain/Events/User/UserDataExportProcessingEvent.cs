using System;
using Newtonsoft.Json;
using Sheaft.Domain.Common;

namespace Sheaft.Domain.Events.User
{
    public class UserDataExportProcessingEvent : DomainEvent
    {
        [JsonConstructor]
        public UserDataExportProcessingEvent(Guid jobId)
        {
            JobId = jobId;
        }

        public Guid JobId { get; }
    }
}
