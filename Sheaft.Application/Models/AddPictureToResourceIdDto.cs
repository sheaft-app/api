using System;
using System.Collections;

namespace Sheaft.Application.Models
{
    public class AddPictureToResourceIdDto
    {
        public Guid Id { get; set; }
        public PictureSourceDto Picture { get; set; }
    }
}