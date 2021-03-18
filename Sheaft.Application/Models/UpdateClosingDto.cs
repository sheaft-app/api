using System;

namespace Sheaft.Application.Models
{
    public class UpdateClosingDto : CreateClosingDto
    {
        public Guid Id { get; set; }
    }
}