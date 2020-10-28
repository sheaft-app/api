using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sheaft.Core;
using Sheaft.Core.Extensions;
using Sheaft.Core.Models;
using Sheaft.Domain.Models;
using Sheaft.Exceptions;
using Sheaft.Domain.Enums;
using Sheaft.Options;
using Sheaft.Application.Interop;
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

namespace Sheaft.Infrastructure.Services
{
    public class PictureService : BaseService, IPictureService
    {
        private readonly HttpClient _httpClient;
        private readonly StorageOptions _storageOptions;
        private readonly IBlobService _blobService;

        public PictureService(
            IHttpClientFactory httpClientFactory,
            IOptionsSnapshot<StorageOptions> storageOptions,
            IBlobService blobService,
            ILogger<PictureService> logger) : base(logger)
        {
            _httpClient = httpClientFactory.CreateClient("pictures");
            _storageOptions = storageOptions.Value;
            _blobService = blobService;
        }

        public async Task<Result<string>> HandleUserPictureAsync(User user, string picture, CancellationToken token)
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

                        return await _blobService.UploadUserPictureAsync(user.Id, blobStream.ToArray(), token);
                    }
                }
            });
        }

        public async Task<Result<string>> HandleTagPictureAsync(Tag tag, string picture, CancellationToken token)
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

                        return await _blobService.UploadTagPictureAsync(tag.Id, blobStream.ToArray(), token);
                    }
                }
            });
        }

        public async Task<Result<string>> HandleProductPictureAsync(Product entity, string picture, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                if (string.IsNullOrWhiteSpace(picture) && string.IsNullOrWhiteSpace(entity.Picture))
                {
                    return Ok(GetDefaultProductPicture(entity.Tags?.Select(t => t.Tag)));
                }

                if (string.IsNullOrWhiteSpace(picture))
                    return Ok(entity.Picture);

                var bytes = await RetrievePictureBytesAsync(picture);
                if (bytes == null)
                    return Ok(entity.Picture);

                var imageId = Guid.NewGuid().ToString("N");
                using (Image image = Image.Load(bytes))
                {
                    await UploadPictureAsync(image, entity.Producer.Id, entity.Id, imageId, PictureSize.LARGE, 620, 256, token, quality: 50);
                    await UploadPictureAsync(image, entity.Producer.Id, entity.Id, imageId, PictureSize.MEDIUM, 310, 128, token, quality: 70);
                    await UploadPictureAsync(image, entity.Producer.Id, entity.Id, imageId, PictureSize.SMALL, 64, 64, token, ResizeMode.Crop, quality: 90);
                    await UploadPictureAsync(image, entity.Producer.Id, entity.Id, imageId, PictureSize.ORIGINAL, token);
                }

                return Ok($"{_storageOptions.ContentScheme}://{_storageOptions.ContentHostname}/{_storageOptions.Containers.Pictures}/{CoreProductExtensions.GetPictureUrl(entity.Producer.Id, entity.Id, imageId)}");
            });
        }

        private string GetDefaultProductPicture(IEnumerable<Tag> tags)
        {
            var category = tags.FirstOrDefault(t => t.Kind == TagKind.Category);
            if (category != null)
                return $"{_storageOptions.ContentScheme}://{_storageOptions.ContentHostname}/{_storageOptions.Containers.Pictures}/products/categories/{category.Id.ToString("D").ToUpperInvariant()}";

            return $"{_storageOptions.ContentScheme}://{_storageOptions.ContentHostname}/{_storageOptions.Containers.Pictures}/products/categories/default";
        }

        private async Task<Result<string>> UploadPictureAsync(Image image, Guid userId, Guid productId, string filename, string size, int width, int height, CancellationToken token, ResizeMode mode = ResizeMode.Max, int quality = 100)
        {
            using (var blobStream = new MemoryStream())
            {
                image.Clone(context => context.Resize(new ResizeOptions
                {
                    Mode = mode,
                    Size = new Size(width, height),
                    Compand = true,
                    Sampler = KnownResamplers.Lanczos3
                })).Save(blobStream, new JpegEncoder { Quality = quality, Subsample = JpegSubsample.Ratio444 });

                return await _blobService.UploadProductPictureAsync(userId, productId, filename, size, blobStream.ToArray(), token);
            }
        }

        private async Task<Result<string>> UploadPictureAsync(Image image, Guid userId, Guid productId, string filename, string size, CancellationToken token)
        {
            using (var blobStream = new MemoryStream())
            {
                image.Save(blobStream, new JpegEncoder { Quality = 100, Subsample = JpegSubsample.Ratio444 });
                return await _blobService.UploadProductPictureAsync(userId, productId, filename, size, blobStream.ToArray(), token);
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

            if ((picture.StartsWith("http") || picture.StartsWith("https")) && !picture.StartsWith($"{_storageOptions.ContentScheme}://{_storageOptions.ContentHostname}"))
            {
                using (var response = await _httpClient.GetAsync(picture))
                    return await response.Content.ReadAsByteArrayAsync();
            }

            return null;
        }
    }
}
