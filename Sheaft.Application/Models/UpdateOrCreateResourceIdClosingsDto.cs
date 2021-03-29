using System;
using System.Collections.Generic;

namespace Sheaft.Application.Models
{
    public class UpdateOrCreateResourceIdClosingsDto
    {
        public Guid Id { get; set; }
        public List<UpdateOrCreateClosingDto> Closings { get; set; }
    }
    public class UpdateOrCreateResourceIdClosingDto
    {
        public Guid Id { get; set; }
        public UpdateOrCreateClosingDto Closing { get; set; }
    }
}