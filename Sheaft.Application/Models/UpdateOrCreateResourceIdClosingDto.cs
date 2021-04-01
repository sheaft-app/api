using System;

namespace Sheaft.Application.Models
{
    public class UpdateOrCreateResourceIdClosingDto
    {
        public Guid Id { get; set; }
        public UpdateOrCreateClosingDto Closing { get; set; }
    }
}