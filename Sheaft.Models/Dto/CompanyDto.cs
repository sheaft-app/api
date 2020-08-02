using Sheaft.Interop.Enums;
using System;
using System.Collections.Generic;

namespace Sheaft.Models.Dto
{
    public class CompanyDto
    {
        public Guid Id { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public DateTimeOffset? RemovedOn { get; set; }
        public ProfileKind Kind { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Reason { get; set; }
        public string Picture { get; set; }
        public string Description { get; set; }
        public string VatIdentifier { get; set; }
        public string Siret { get; set; }
        public bool AppearInBusinessSearchResults { get; set; }
        public AddressDto Address { get; set; }
        public IEnumerable<TagDto> Tags { get; set; }
        public IEnumerable<TimeSlotDto> OpeningHours { get; set; }
    }
}