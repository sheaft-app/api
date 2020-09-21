using Sheaft.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Sheaft.Application.Models
{
    public class CreateBusinessLegalInput : BusinessLegalInput
    {
        public Guid UserId { get; set; }
    }
}