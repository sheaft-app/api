using Sheaft.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Sheaft.Application.Models
{
    public class UpdateBusinessLegalInput : BusinessLegalInput
    {
        public Guid Id { get; set; }
    }
}