using System;

namespace Sheaft.Application.Models
{
    public class UpdateReturnableDto : CreateReturnableDto
    {
        public Guid Id { get; set; }
    }
}