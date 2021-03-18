using System;

namespace Sheaft.Application.Models
{
    public class UpdateBusinessLegalDto : CreateBusinessLegalDto
    {
        public Guid Id { get; set; }
    }
}