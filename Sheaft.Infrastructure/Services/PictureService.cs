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
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Models;
using Sheaft.Application.Common.Options;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using SixLabors.ImageSharp.Formats.Png;

namespace Sheaft.Infrastructure.Services
{
    public class PictureService : BaseService, IPictureService
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

        public async Task<Result<string>> HandleUserPictureAsync(User user, string picture, string originalPicture,
            CancellationToken token)
        {
            var bytes = await RetrievePictureBytesAsync(picture);
            if (bytes == null)
                return Success(picture);

            var originalBytes = await RetrievePictureBytesAsync(originalPicture);
            if (originalBytes != null)
                using (var originalImage = Image.Load(originalBytes))
                    await UploadUserPictureAsync(originalImage, user.Id, PictureSize.ORIGINAL, token);

            using (var image = Image.Load(bytes))
                return await UploadUserPictureAsync(image, user.Id, PictureSize.MEDIUM,
                    _pictureOptions.User.Medium.Width, _pictureOptions.User.Medium.Height, token,
                    quality: _pictureOptions.User.Medium.Quality);
        }

        private async Task<Result<string>> UploadUserPictureAsync(Image image, Guid userId, string size, int width,
            int height, CancellationToken token, ResizeMode mode = ResizeMode.Max,
            int quality = 100)
        {
            using (var blobStream = new MemoryStream())
            {
                await image.Clone(context => context.Resize(new ResizeOptions
                {
                    Mode = mode,
                    Size = new Size(width, height),
                    Compand = true,
                    Sampler = KnownResamplers.Lanczos3
                })).SaveAsync(blobStream,
                    new PngEncoder {CompressionLevel = PngCompressionLevel.BestCompression, IgnoreMetadata = true},
                    token);

                return await _blobService.UploadUserPreviewAsync(userId, size, blobStream.ToArray(), token);
            }
        }

        private async Task<Result<string>> UploadUserPictureAsync(Image image, Guid userId, string size,
            CancellationToken token)
        {
            using (var blobStream = new MemoryStream())
            {
                await image.SaveAsync(blobStream,
                    new PngEncoder {CompressionLevel = PngCompressionLevel.BestCompression, IgnoreMetadata = true},
                    token);
                
                return await _blobService.UploadUserPreviewAsync(userId, size, blobStream.ToArray(), token);
            }
        }

        private async Task<Result<string>> UploadProfilePictureAsync(Image image, Guid userId, Guid pictureId, CancellationToken token)
        {
            using (var blobStream = new MemoryStream())
            {
                await image.SaveAsync(blobStream,
                    new PngEncoder {CompressionLevel = PngCompressionLevel.BestCompression, IgnoreMetadata = true},
                    token);
                
                return await _blobService.UploadProfilePictureAsync(userId, pictureId, blobStream.ToArray(), token);
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

        public async Task<Result<string>> HandleProductPictureAsync(Product product, Guid pictureId, string picture, CancellationToken token)
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
                            {CompressionLevel = PngCompressionLevel.BestCompression, IgnoreMetadata = true}, token);

                    return await _blobService.UploadProductPictureAsync(product.Producer.Id, product.Id, pictureId, blobStream.ToArray(), token);
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

        public async Task<Result<string>> HandleProfilePictureAsync(User user, Guid pictureId, string picture,
            CancellationToken token)
        {
            var bytes = await RetrievePictureBytesAsync(picture);
            if (bytes == null)
                return Success(picture);

            using (var image = Image.Load(bytes))
                return await UploadProfilePictureAsync(image, user.Id, pictureId, token);
        }

        public async Task<Result<string>> HandleProductPreviewAsync(Product entity, string pictureResized,
            string originalPicture, CancellationToken token)
        {
            if (string.IsNullOrWhiteSpace(pictureResized) && string.IsNullOrWhiteSpace(entity.Picture))
                return Success(GetDefaultProductPicture(entity.Tags?.Select(t => t.Tag)));

            if (string.IsNullOrWhiteSpace(pictureResized))
                return Success(entity.Picture);

            var bytes = await RetrievePictureBytesAsync(pictureResized);
            if (bytes == null)
                return Success(entity.Picture);

            var originalBytes = await RetrievePictureBytesAsync(originalPicture);
            if (originalBytes != null)
                using (Image image = Image.Load(originalBytes))
                    await UploadProductOriginalPreviewAsync(image, entity.Producer.Id, entity.Id, token);
            
            using (Image image = Image.Load(bytes))
            {
                await UploadProductPreviewAsync(image, entity.Producer.Id, entity.Id, PictureSize.SMALL,
                    _pictureOptions.Product.Small.Width, _pictureOptions.Product.Small.Height, token,
                    quality: _pictureOptions.Product.Small.Quality);
                
                await UploadProductPreviewAsync(image, entity.Producer.Id, entity.Id, PictureSize.MEDIUM,
                    _pictureOptions.Product.Medium.Width, _pictureOptions.Product.Medium.Height, token,
                    quality: _pictureOptions.Product.Medium.Quality);
                
                return await UploadProductPreviewAsync(image, entity.Producer.Id, entity.Id, PictureSize.LARGE,
                    _pictureOptions.Product.Large.Width, _pictureOptions.Product.Large.Height, token,
                    quality: _pictureOptions.Product.Large.Quality);
            }
        }

        private string GetDefaultProductPicture(IEnumerable<Tag> tags)
        {
            var category = tags.FirstOrDefault(t => t.Kind == TagKind.Category);
            return category?.Picture;
        }

        private async Task<Result<string>> UploadProductPreviewAsync(Image image, Guid userId, Guid productId,
            string size, int width, int height, CancellationToken token,
            ResizeMode mode = ResizeMode.Max, int quality = 100)
        {
            using (var blobStream = new MemoryStream())
            {
                await image.Clone(context => context.Resize(new ResizeOptions
                {
                    Mode = mode,
                    Size = new Size(width, height),
                    Compand = true,
                    Sampler = KnownResamplers.Lanczos3
                })).SaveAsync(blobStream,
                    new PngEncoder {CompressionLevel = PngCompressionLevel.BestCompression, IgnoreMetadata = true},
                    token);

                return await _blobService.UploadProductPreviewAsync(userId, productId, size, blobStream.ToArray(), token);
            }
        }

        private async Task<Result<string>> UploadProductOriginalPreviewAsync(Image image, Guid userId, Guid productId, CancellationToken token)
        {
            using (var blobStream = new MemoryStream())
            {
                await image.SaveAsync(blobStream,
                    new PngEncoder {CompressionLevel = PngCompressionLevel.BestCompression, IgnoreMetadata = true},
                    token);
                return await _blobService.UploadProductPreviewAsync(userId, productId, PictureSize.ORIGINAL,
                    blobStream.ToArray(), token);
            }
        }

        private async Task<byte[]> RetrievePictureBytesAsync(string picture)
        {
            if (string.IsNullOrWhiteSpace(picture))
                return null;

            if (!picture.StartsWith("http") && !picture.StartsWith("https"))
            {
                var base64Data = picture.StartsWith("data:image")
                    ? Regex.Match(picture, @"data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value
                    : picture;
                return Convert.FromBase64String(base64Data);
            }

            if ((picture.StartsWith("http") || picture.StartsWith("https")) &&
                !picture.StartsWith($"{_storageOptions.ContentScheme}://{_storageOptions.ContentHostname}"))
            {
                using (var response = await _httpClient.GetAsync(picture))
                    return await response.Content.ReadAsByteArrayAsync();
            }

            return null;
        }
    }
}