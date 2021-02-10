using System;
using Newtonsoft.Json;
using Sheaft.Core;
using Sheaft.Domain.Models.Common;

namespace Sheaft.Application.Events
{
    public class UserDataExportProcessingEvent : DomainEvent
    {
        [JsonConstructor]
        public UserDataExportProcessingEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid JobId { get; set; }
    }
}
