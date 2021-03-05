using System;

namespace Sheaft.Application.Common.Models.Inputs
{
    public class UpdatePictureInput
    {
        public Guid Id { get; set; }
        public PictureInput Picture { get; set; }
    }
}