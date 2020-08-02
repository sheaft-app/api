using System;
using System.Collections.Generic;
using Sheaft.Models.Inputs;

namespace Sheaft.Application.Commands
{
    public class RegisterCompanyCommand : Command<Guid>
    {
        public RegisterCompanyCommand(Interop.IRequestUser user) : base(user)
        {
        }

        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Description { get; set; }
        public string VatIdentifier { get; set; }
        public string Picture { get; set; }
        public string Siret { get; set; }
        public AddressInput Address { get; set; }
        public RegisterOwnerInput Owner { get; set; }
        public bool AppearInBusinessSearchResults { get; set; }
        public IEnumerable<Guid> Tags { get; set; }
        public IEnumerable<TimeSlotGroupInput> OpeningHours { get; set; }
    }
}
