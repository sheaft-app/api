using Sheaft.Models.Inputs;
using System;
using System.Collections.Generic;
using Sheaft.Interop;

namespace Sheaft.Application.Commands
{
    public class AcceptAgreementCommand : Command<bool>
    {
        public AcceptAgreementCommand(IRequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
        public IEnumerable<TimeSlotGroupInput> SelectedHours { get; set; }
    }
}
