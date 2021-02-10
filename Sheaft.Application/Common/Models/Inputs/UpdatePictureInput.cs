using System;

namespace Sheaft.Application.Common.Models.Inputs
{
    public class UpdatePictureInput
    {
        public Guid Id { get; set; }
        public string OriginalPicture { get; set; }
        public string Picture { get; set; }
    }
}