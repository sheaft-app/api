using System;

namespace Sheaft.Application.Models
{
    public class MarkMyNotificationAsReadDto
    {
        public MarkMyNotificationAsReadDto(DateTimeOffset readBefore)
        {
            ReadBefore = readBefore;
        }
        
        public DateTimeOffset ReadBefore { get; set; }
    }
}