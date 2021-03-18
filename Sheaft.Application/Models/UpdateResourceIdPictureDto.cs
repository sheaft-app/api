using System;

namespace Sheaft.Application.Models
{
    public class UpdateResourceIdPictureDto
    {
        public Guid Id { get; set; }
        public PictureSourceDto Picture { get; set; }
    }
}