using System;

namespace Sheaft.Application.Common.Extensions
{
    public static class ProductExtensions
    {
        public static string GetPictureUrl(Guid userId, Guid productId, string filename, string size)
        {
            if (string.IsNullOrWhiteSpace(filename))
                return null;

            return $"users/{userId:N}/products/{productId:N}/{filename}_{size}.jpg";
        }
        
        public static string GetPictureUrl(Guid userId, Guid productId, string filename)
        {
            if (string.IsNullOrWhiteSpace(filename))
                return null;

            return $"users/{userId:N}/products/{productId:N}/{filename}";
        }
        
        public static string GetPictureUrl(string image, string size)
        {
            if (string.IsNullOrWhiteSpace(image))
                return null;

            if (image.EndsWith(".jpg") || image.EndsWith(".jpeg") || image.EndsWith(".png"))
                return image;

            return $"{image}_{size}.jpg";
        }
    }
}