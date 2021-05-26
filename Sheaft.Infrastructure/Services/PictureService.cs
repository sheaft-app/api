using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Services;
using Sheaft.Core;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Options;
using SixLabors.ImageSharp.Formats.Png;

namespace Sheaft.Infrastructure.Services
{
    public class PictureService : SheaftService, IPictureService
    {
        private readonly HttpClient _httpClient;
        private readonly StorageOptions _storageOptions;
        private readonly PictureOptions _pictureOptions;
        private readonly IBlobService _blobService;

        public PictureService(
            IHttpClientFactory httpClientFactory,
            IOptionsSnapshot<StorageOptions> storageOptions,
            IOptionsSnapshot<PictureOptions> pictureOptions,
            IBlobService blobService,
            ILogger<PictureService> logger) : base(logger)
        {
            _httpClient = httpClientFactory.CreateClient("pictures");
            _storageOptions = storageOptions.Value;
            _pictureOptions = pictureOptions.Value;
            _blobService = blobService;
        }

        public async Task<Result<string>> HandleUserPictureAsync(User entity, Guid pictureId, string picture, CancellationToken token)
        {
            var bytes = await RetrievePictureBytesAsync(picture);
            if (bytes == null)
                return Success(picture);

            using (var image = Image.Load(bytes))
            {
                await UploadUserPreviewAsync(image, entity.Id, pictureId, PictureSize.SMALL, token);
                await UploadUserPreviewAsync(image, entity.Id, pictureId, PictureSize.MEDIUM, token);
                return await UploadUserPreviewAsync(image, entity.Id, pictureId, PictureSize.LARGE, token);
            }
        }

        public async Task<Result<string>> HandleUserProfileAsync(User entity, string picture, CancellationToken token)
        {
            var bytes = await RetrievePictureBytesAsync(picture);
            if (bytes == null)
                return Success(picture);

            using (Image image = Image.Load(bytes))
            {
                using (var blobStream = new MemoryStream())
                {
                    await image.Clone(context => context.Resize(new ResizeOptions
                    {
                        Mode = ResizeMode.Max,
                        Size = new Size(128, 128),
                        Compand = true,
                        Sampler = KnownResamplers.Lanczos3
                    })).SaveAsync(blobStream,
                        new PngEncoder {CompressionLevel = PngCompressionLevel.DefaultCompression, IgnoreMetadata = true},
                        token);

                    return await _blobService.UploadUserProfileAsync(entity.Id, blobStream.ToArray(), token);
                }
            }
        }

        public async Task<Result<string>> HandleTagPictureAsync(Tag tag, string picture, CancellationToken token)
        {
            var bytes = await RetrievePictureBytesAsync(picture);
            if (bytes == null)
                return Success(picture);

            using (var image = Image.Load(bytes))
            {
                using (var blobStream = new MemoryStream())
                {
                    await image.SaveAsync(blobStream,
                        new PngEncoder
                            {CompressionLevel = PngCompressionLevel.DefaultCompression, IgnoreMetadata = true}, token);

                    return await _blobService.UploadTagPictureAsync(tag.Id, blobStream.ToArray(), token);
                }
            }
        }

        public async Task<Result<string>> HandleTagIconAsync(Tag tag, string picture, CancellationToken token)
        {
            var bytes = await RetrievePictureBytesAsync(picture);
            if (bytes == null)
                return Success(picture);

            using (var image = Image.Load(bytes))
            {
                using (var blobStream = new MemoryStream())
                {
                    await image.SaveAsync(blobStream,
                        new PngEncoder
                            {CompressionLevel = PngCompressionLevel.DefaultCompression, IgnoreMetadata = true}, token);

                    return await _blobService.UploadTagIconAsync(tag.Id, blobStream.ToArray(), token);
                }
            }
        }

        public async Task<Result<string>> HandleProductPictureAsync(Product entity, Guid pictureId, string picture,
            CancellationToken token)
        {
            if (string.IsNullOrWhiteSpace(picture) && string.IsNullOrWhiteSpace(entity.Picture))
                return Success(GetDefaultProductPicture(entity.Tags?.Select(t => t.Tag)));

            var bytes = await RetrievePictureBytesAsync(picture);
            if (bytes == null)
                return Success(picture);

            using (Image image = Image.Load(bytes))
            {
                await UploadProductPreviewAsync(image, entity.ProducerId, entity.Id, pictureId, PictureSize.SMALL, token);
                await UploadProductPreviewAsync(image, entity.ProducerId, entity.Id, pictureId, PictureSize.MEDIUM, token);
                return await UploadProductPreviewAsync(image, entity.ProducerId, entity.Id, pictureId, PictureSize.LARGE, token);
            }
        }
        
        private string GetDefaultProductPicture(IEnumerable<Tag> tags)
        {
            var category = tags.FirstOrDefault(t => t.Kind == TagKind.Category);
            return category?.Picture;
        }


        private async Task<Result<string>> UploadUserPreviewAsync(Image image, Guid userId, Guid pictureId,
            string size, CancellationToken token)
        {
            using (var blobStream = new MemoryStream())
            {
                var width = image.Width;
                var height = image.Height;

                switch (size)
                {
                    case PictureSize.MEDIUM:
                        var mediumDivider = GetDivider(2, width, height, _pictureOptions.User.Medium.Width, _pictureOptions.User.Medium.Height);
                        width /= mediumDivider;
                        height /= mediumDivider;
                        break;
                    case PictureSize.SMALL:
                        var smallDivider = GetDivider(4, width, height, _pictureOptions.User.Small.Width, _pictureOptions.User.Small.Height);
                        width /= smallDivider;
                        height /= smallDivider;
                        break;
                }

                await image.Clone(context => context.Resize(new ResizeOptions
                {
                    Mode = ResizeMode.Max,
                    Size = new Size(width, height),
                    Compand = true,
                    Sampler = KnownResamplers.Lanczos3
                })).SaveAsync(blobStream,
                    new PngEncoder {CompressionLevel = PngCompressionLevel.DefaultCompression, IgnoreMetadata = true},
                    token);

                return await _blobService.UploadUserPictureAsync(userId, pictureId, size,
                    blobStream.ToArray(), token);
            }
        }
        
        private async Task<Result<string>> UploadProductPreviewAsync(Image image, Guid userId, Guid productId,
            Guid pictureId, string size, CancellationToken token)
        {
            using (var blobStream = new MemoryStream())
            {
                var width = image.Width;
                var height = image.Height;

                switch (size)
                {
                    case PictureSize.MEDIUM:
                        var mediumDivider = GetDivider(2, width, height, _pictureOptions.Product.Medium.Width, _pictureOptions.Product.Medium.Height);
                        width /= mediumDivider;
                        height /= mediumDivider;
                        break;
                    case PictureSize.SMALL:
                        var smallDivider = GetDivider(4, width, height, _pictureOptions.Product.Small.Width, _pictureOptions.Product.Small.Height);
                        width /= smallDivider;
                        height /= smallDivider;
                        break;
                }

                await image.Clone(context => context.Resize(new ResizeOptions
                {
                    Mode = ResizeMode.Max,
                    Size = new Size(width, height),
                    Compand = true,
                    Sampler = KnownResamplers.Lanczos3
                })).SaveAsync(blobStream,
                    new PngEncoder {CompressionLevel = PngCompressionLevel.DefaultCompression, IgnoreMetadata = true},
                    token);

                return await _blobService.UploadProductPictureAsync(userId, productId, pictureId, size,
                    blobStream.ToArray(), token);
            }
        }

        private int GetDivider(int mediumDivider, int width, int height, int minWidth, int minHeight)
        {
            while (mediumDivider > 1)
            {
                if (width / mediumDivider < minWidth &&
                    height / mediumDivider < minHeight)
                    mediumDivider--;
                else
                    break;
            }

            return mediumDivider;
        }

        private async Task<byte[]> RetrievePictureBytesAsync(string picture)
        {
            if (string.IsNullOrWhiteSpace(picture))
                return null;

            if (!picture.StartsWith("http"))
            {
                var base64Data = picture.StartsWith("data:image")
                    ? Regex.Match(picture, @"data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value
                    : picture;
                return Convert.FromBase64String(base64Data);
            }

            if (picture.StartsWith("http") && !picture.Contains("sheaft.com"))
            {
                using (var response = await _httpClient.GetAsync(picture))
                    return await response.Content.ReadAsByteArrayAsync();
            }

            return null;
        }
    }
}