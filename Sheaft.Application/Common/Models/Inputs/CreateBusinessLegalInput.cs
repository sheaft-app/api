using System;

namespace Sheaft.Application.Common.Models.Inputs
{
    public class CreateBusinessLegalInput : BusinessLegalInput
    {
        public Guid UserId { get; set; }
    }
}