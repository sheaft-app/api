using Sheaft.Domain.Enums;
using System;

namespace Sheaft.Application.Models
{
    public class NationalityViewModel
    {
        public Guid Id { get; set; }
        public string Alpha2 { get; set; }
        public string Name { get; set; }
    }
}
