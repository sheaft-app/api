using System;
using Newtonsoft.Json;
using Sheaft.Core;

namespace Sheaft.Application.Events
{
    public class UserDataExportFailedEvent : Event
    {
        [JsonConstructor]
        public UserDataExportFailedEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid JobId { get; set; }
    }
}
