using Sheaft.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Sheaft.Application.Models
{
    public class BaseLegalDto
    {
        public Guid Id { get; set; }
        public OwnerDto Owner { get; set; }
    }
}