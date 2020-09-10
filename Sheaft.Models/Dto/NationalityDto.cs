using Sheaft.Interop.Enums;
using System;

namespace Sheaft.Models.Dto
{
    public class NationalityDto
    {
        public Guid Id { get; set; }
        public CountryIsoCode Code { get { return (CountryIsoCode)Enum.Parse(typeof(CountryIsoCode), Alpha2); } }
        public string Alpha2 { get; set; }
        public string Name { get; set; }
    }
}