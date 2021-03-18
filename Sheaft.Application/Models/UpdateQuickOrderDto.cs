using System;

namespace Sheaft.Application.Models
{
    public class UpdateQuickOrderDto : CreateQuickOrderDto
    {
        public Guid Id { get; set; }
    }
}