using System;
using Newtonsoft.Json;
using Sheaft.Domain.Common;

namespace Sheaft.Domain.Events.User
{
    public class UserDataExportSucceededEvent : DomainEvent
    {
        [JsonConstructor]
        public UserDataExportSucceededEvent(Guid jobId)
        {
            JobId = jobId;
        }

        public Guid JobId { get; }
    }
}
