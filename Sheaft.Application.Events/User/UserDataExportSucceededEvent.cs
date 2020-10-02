using Sheaft.Core;
using System;
using Newtonsoft.Json;

namespace Sheaft.Application.Events
{
    public class UserDataExportSucceededEvent : Event
    {
        [JsonConstructor]
        public UserDataExportSucceededEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid JobId { get; set; }
    }
}
