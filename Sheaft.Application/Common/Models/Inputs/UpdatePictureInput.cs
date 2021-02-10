using System;

namespace Sheaft.Application.Models
{
    public class UpdatePictureInput
    {
        public Guid Id { get; set; }
        public string OriginalPicture { get; set; }
        public string Picture { get; set; }
    }
}