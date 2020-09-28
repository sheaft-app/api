using Sheaft.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Sheaft.Application.Models
{
    public class CountryViewModel
    {
        public Guid Id { get; set; }
        public string Alpha2 { get; set; }
        public string Name { get; set; }
    }
}
