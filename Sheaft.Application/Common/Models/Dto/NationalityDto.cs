using Sheaft.Domain.Enums;
using System;

namespace Sheaft.Application.Models
{
    public class NationalityDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
}