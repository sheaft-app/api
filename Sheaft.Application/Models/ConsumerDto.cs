using System.Collections.Generic;

namespace Sheaft.Application.Models
{
    public class ConsumerDto : UserDto
    {
        public bool Anonymous { get; set; }
        public IEnumerable<PictureDto> Pictures { get; set; }
    }
}