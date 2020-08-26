using Sheaft.Models.Inputs;
using System;
using System.Collections.Generic;
using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Commands
{
    public class AcceptAgreementCommand : Command<bool>
    {
        [JsonConstructor]
        public AcceptAgreementCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
        public IEnumerable<TimeSlotGroupInput> SelectedHours { get; set; }
    }
}
