using System;

namespace Sheaft.Application.Common.Models.Inputs
{
    public class AddPictureToInput
    {
        public Guid Id { get; set; }
        public PictureInput Picture { get; set; }
    }
}