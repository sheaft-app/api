using Sheaft.Models.Inputs;
using System;
using System.Collections.Generic;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class AcceptAgreementCommand : Command<bool>
    {
        public AcceptAgreementCommand(RequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
        public IEnumerable<TimeSlotGroupInput> SelectedHours { get; set; }
    }
}
