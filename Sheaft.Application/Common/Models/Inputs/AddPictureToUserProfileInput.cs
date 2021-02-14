using System;

namespace Sheaft.Application.Common.Models.Inputs
{
    public class AddPictureToUserProfileInput
    {
        public Guid Id { get; set; }
        public PictureInput Picture { get; set; }
    }
}