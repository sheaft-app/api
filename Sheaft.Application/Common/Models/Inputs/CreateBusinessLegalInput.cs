using System;

namespace Sheaft.Application.Models
{
    public class CreateBusinessLegalInput : BusinessLegalInput
    {
        public Guid UserId { get; set; }
    }
}