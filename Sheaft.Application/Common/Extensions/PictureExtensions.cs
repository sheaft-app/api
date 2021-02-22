using System;

namespace Sheaft.Application.Common.Extensions
{
    public static class PictureExtensions
    {
        public static string GetPictureUrl(Guid pictureId, string image, string size)
        {
            if (string.IsNullOrWhiteSpace(image))
                return null;

            if (image.EndsWith(".jpg") || image.EndsWith(".jpeg") || image.EndsWith(".png"))
                return image;

            return image.EndsWith($"{pictureId:N}") ? $"{image}_{size}.jpg" : $"{image}_{size}.png";
        }
    }
}