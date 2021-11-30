using System;
using Newtonsoft.Json;
using Sheaft.Domain.Common;

namespace Sheaft.Domain.Events.User
{
    public class UserDataExportFailedEvent : DomainEvent
    {
        [JsonConstructor]
        public UserDataExportFailedEvent(Guid jobId)
        {
            JobId = jobId;
        }

        public Guid JobId { get; }
    }
}
