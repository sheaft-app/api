using System;
using System.Collections.Generic;

namespace Sheaft.Application.Models
{
    public class CreateResourceIdClosingsDto
    {
        public Guid Id { get; set; }
        public List<CreateClosingDto> Closings { get; set; }
    }
}