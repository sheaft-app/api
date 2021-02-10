using System;
using Sheaft.Domain.Enum;

namespace Sheaft.Application.Common.Models.Inputs
{
    public class UboInput
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTimeOffset BirthDate { get; set; }
        public CountryIsoCode Nationality { get; set; }
        public AddressInput Address { get; set; }
        public BirthAddressInput BirthPlace { get; set; }
    }

    public class CreateUboInput : UboInput
    {
    }

    public class UpdateUboInput: UboInput
    {
        public Guid Id { get; set; }
    }
}