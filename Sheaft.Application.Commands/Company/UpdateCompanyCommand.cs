using System;
using System.Collections.Generic;
using Sheaft.Models.Inputs;

namespace Sheaft.Application.Commands
{
    public class UpdateCompanyCommand : Command<bool>
    {
        public UpdateCompanyCommand(Interop.IRequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Description { get; set; }
        public string Siret { get; set; }
        public string VatIdentifier { get; set; }
        public bool AppearInBusinessSearchResults { get; set; }
        public AddressInput Address { get; set; }
        public IEnumerable<Guid> Tags { get; set; }
        public IEnumerable<TimeSlotGroupInput> OpeningHours { get; set; }
    }
}
