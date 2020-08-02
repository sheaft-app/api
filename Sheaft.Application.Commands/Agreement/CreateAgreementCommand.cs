using Sheaft.Models.Inputs;
using System;
using System.Collections.Generic;
using Sheaft.Interop;

namespace Sheaft.Application.Commands
{
    public class CreateAgreementCommand : Command<Guid>
    {
        public CreateAgreementCommand(IRequestUser user) : base(user)
        {
        }

        public Guid StoreId { get; set; }
        public Guid DeliveryModeId { get; set; }
        public IEnumerable<TimeSlotGroupInput> SelectedHours { get; set; }
    }
}
