using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sheaft.Core;
using Sheaft.Core.Extensions;
using Sheaft.Core.Models;
using Sheaft.Domain.Models;
using Sheaft.Exceptions;
using Sheaft.Interop.Enums;
using Sheaft.Options;
using Sheaft.Services.Interop;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Sheaft.Services
{
    public class ImageService : ResultsHandler, IImageService
    {
        private readonly HttpClient _httpClient;
        private readonly StorageOptions _storageOptions;
        private readonly IBlobService _blobService;

        public ImageService(
            IHttpClientFactory httpClientFactory,
            IOptionsSnapshot<StorageOptions> storageOptions,
            IBlobService blobService,
            ILogger<ImageService> logger) : base(logger)
        {
            _httpClient = httpClientFactory.CreateClient("images");
            _storageOptions = storageOptions.Value;
            _blobService = blobService;
        }

        public async Task<Result<string>> HandleUserImageAsync(Guid id, string picture, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var bytes = await RetrievePictureBytesAsync(picture);
                if (bytes == null)
                    return Ok(picture);

                using (var image = Image.Load(bytes))
                {
                    using (var blobStream = new MemoryStream())
                    {
                        image.Clone(context => context.Resize(new ResizeOptions
                        {
                            Mode = ResizeMode.Crop,
                            Size = new Size(64, 64)
                        })).Save(blobStream, new JpegEncoder { Quality = 100 });

                        var compImage = await _blobService.UploadUserPictureAsync(id, blobStream, token);
                        if (!compImage.Success)
                            throw compImage.Exception ?? new BadRequestException();

                        return compImage;
                    }
                }
            });
        }

        public async Task<Result<string>> HandleProductImageAsync(Product entity, string picture, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                if (string.IsNullOrWhiteSpace(picture) && string.IsNullOrWhiteSpace(entity.Image))
                {
                    return Ok(GetDefaultProductImage(entity.Tags?.Select(t => t.Tag)));
                }

                if (string.IsNullOrWhiteSpace(picture))
                    return Ok((string)null);

                byte[] bytes = null;
                if (!picture.StartsWith("http") && !picture.StartsWith("https"))
                {
                    var base64Data = picture.StartsWith("data:image") ? Regex.Match(picture, @"data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value : picture;
                    bytes = Convert.FromBase64String(base64Data);
                }
                else if (!picture.StartsWith($"https://{_storageOptions.Account}.blob.{_storageOptions.Suffix}"))
                {
                    using (var response = await _httpClient.GetAsync(picture))
                        bytes = await response.Content.ReadAsByteArrayAsync();
                }
                else
                {
                    return Ok(picture);
                }

                var imageId = Guid.NewGuid().ToString("N");
                using (Image image = Image.Load(bytes))
                {
                    await UploadImageAsync(image, entity.Producer.Id, entity.Id, imageId, ImageSize.LARGE, 620, 256, token);
                    await UploadImageAsync(image, entity.Producer.Id, entity.Id, imageId, ImageSize.MEDIUM, 310, 128, token);
                    await UploadImageAsync(image, entity.Producer.Id, entity.Id, imageId, ImageSize.SMALL, 64, 64, token, ResizeMode.Crop);
                }

                return Ok($"https://{_storageOptions.Account}.blob.{_storageOptions.Suffix}/{_storageOptions.Containers.Pictures}/{CoreProductExtensions.GetImageUrl(entity.Producer.Id, entity.Id, imageId)}");
            });
        }

        private string GetDefaultProductImage(IEnumerable<Tag> tags)
        {
            var category = tags.FirstOrDefault(t => t.Kind == TagKind.Category);
            if (category != null)
                return $"https://{_storageOptions.Account}.blob.{_storageOptions.Suffix}/{_storageOptions.Containers.Pictures}/products/categories/{category.Id.ToString("D").ToUpperInvariant()}.jpg";

            return $"https://{_storageOptions.Account}.blob.{_storageOptions.Suffix}/{_storageOptions.Containers.Pictures}/products/categories/default.jpg";
        }

        private async Task UploadImageAsync(Image image, Guid userId, Guid productId, string filename, string size, int width, int height, CancellationToken token, ResizeMode mode = ResizeMode.Max, int quality = 100)
        {
            using (var blobStream = new MemoryStream())
            {
                image.Clone(context => context.Resize(new ResizeOptions
                {
                    Mode = mode,
                    Size = new Size(width, height)
                })).Save(blobStream, new JpegEncoder { Quality = quality });

                await _blobService.UploadProductPictureAsync(userId, productId, filename, size, blobStream, token);
            }
        }

        private async Task<byte[]> RetrievePictureBytesAsync(string picture)
        {
            if (string.IsNullOrWhiteSpace(picture))
                return null;

            if (!picture.StartsWith("http") && !picture.StartsWith("https"))
            {
                var base64Data = picture.StartsWith("data:image") ? Regex.Match(picture, @"data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value : picture;
                return Convert.FromBase64String(base64Data);
            }

            if ((picture.StartsWith("http") || picture.StartsWith("https")) && !picture.StartsWith($"https://{_storageOptions.Account}.blob.{_storageOptions.Suffix}"))
            {
                using (var response = await _httpClient.GetAsync(picture))
                    return await response.Content.ReadAsByteArrayAsync();
            }

            return null;
        }
    }
}
