using Sheaft.Interop.Enums;
using System;

namespace Sheaft.Models.Dto
{
    public class CountryDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
}