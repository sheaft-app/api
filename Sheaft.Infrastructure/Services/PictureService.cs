using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
using Sheaft.Application.Common.Extensions;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Models;
using Sheaft.Application.Common.Options;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

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

            var imageId = Guid.NewGuid().ToString("N");

            var originalBytes = await RetrievePictureBytesAsync(originalPicture);
            if (originalBytes != null)
                using (var originalImage = Image.Load(originalBytes))
                    await UploadUserPictureAsync(originalImage, user.Id, imageId, PictureSize.ORIGINAL, token);

            using (var image = Image.Load(bytes))
                return await UploadUserPictureAsync(image, user.Id, imageId, PictureSize.MEDIUM,
                    _pictureOptions.User.Medium.Width, _pictureOptions.User.Medium.Height, token,
                    quality: _pictureOptions.User.Medium.Quality);
        }

        private async Task<Result<string>> UploadUserPictureAsync(Image image, Guid userId, string filename,
            string size, int width, int height, CancellationToken token, ResizeMode mode = ResizeMode.Max,
            int quality = 100)
        {
            using (var blobStream = new MemoryStream())
            {
                image.Clone(context => context.Resize(new ResizeOptions
                {
                    Mode = mode,
                    Size = new Size(width, height),
                    Compand = true,
                    Sampler = KnownResamplers.Lanczos3
                })).Save(blobStream, new JpegEncoder {Quality = quality, Subsample = JpegSubsample.Ratio444});

                return await _blobService.UploadUserPictureAsync(userId, filename, size, blobStream.ToArray(), token);
            }
        }

        private async Task<Result<string>> UploadUserPictureAsync(Image image, Guid userId, string filename,
            string size, CancellationToken token)
        {
            using (var blobStream = new MemoryStream())
            {
                image.Save(blobStream, new JpegEncoder {Quality = 100, Subsample = JpegSubsample.Ratio444});
                return await _blobService.UploadUserPictureAsync(userId, filename, size, blobStream.ToArray(), token);
            }
        }

        public async Task<Result<string>> HandleTagPictureAsync(Tag tag, string picture, CancellationToken token)
        {
            var bytes = await RetrievePictureBytesAsync(picture);
            if (bytes == null)
                return Success(picture);

            var filename = Guid.NewGuid().ToString("N");

            using (var image = Image.Load(bytes))
            {
                await UploadTagPictureAsync(image, tag.Id, filename, PictureSize.LARGE,
                    _pictureOptions.Tag.Large.Width, _pictureOptions.Tag.Large.Height, token,
                    quality: _pictureOptions.Tag.Large.Quality);
                await UploadTagPictureAsync(image, tag.Id, filename, PictureSize.MEDIUM,
                    _pictureOptions.Tag.Medium.Width, _pictureOptions.Tag.Medium.Height, token,
                    quality: _pictureOptions.Tag.Medium.Height);
                await UploadTagPictureAsync(image, tag.Id, filename, PictureSize.SMALL,
                    _pictureOptions.Tag.Small.Width, _pictureOptions.Tag.Small.Height, token,
                    quality: _pictureOptions.Tag.Small.Height);
            }

            return Success(
                $"{_storageOptions.ContentScheme}://{_storageOptions.ContentHostname}/{_storageOptions.Containers.Pictures}/tags/pictures/{tag.Id:N}/{filename}");
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
                    image.Clone(context => context.Resize(new ResizeOptions
                    {
                        Mode = ResizeMode.Crop,
                        Size = new Size(64, 64),
                        Compand = true,
                        Sampler = KnownResamplers.Lanczos3
                    })).Save(blobStream, new JpegEncoder {Quality = 100, Subsample = JpegSubsample.Ratio444});

                    return await _blobService.UploadTagIconAsync(tag.Id, blobStream.ToArray(), token);
                }
            }
        }

        public async Task<Result<string>> HandleProductPictureAsync(Product entity, string picture,
            string originalPicture, CancellationToken token)
        {
            if (string.IsNullOrWhiteSpace(picture) && string.IsNullOrWhiteSpace(entity.Picture))
            {
                return Success(GetDefaultProductPicture(entity.Tags?.Select(t => t.Tag)));
            }

            if (string.IsNullOrWhiteSpace(picture))
                return Success(entity.Picture);

            var bytes = await RetrievePictureBytesAsync(picture);
            if (bytes == null)
                return Success(entity.Picture);

            var originalBytes = await RetrievePictureBytesAsync(originalPicture);

            var imageId = Guid.NewGuid().ToString("N");
            using (Image image = Image.Load(bytes))
            {
                await UploadProductPictureAsync(image, entity.Producer.Id, entity.Id, imageId, PictureSize.LARGE,
                    _pictureOptions.Product.Large.Width, _pictureOptions.Product.Large.Height, token,
                    quality: _pictureOptions.Product.Large.Quality);
                await UploadProductPictureAsync(image, entity.Producer.Id, entity.Id, imageId, PictureSize.MEDIUM,
                    _pictureOptions.Product.Medium.Width, _pictureOptions.Product.Medium.Height, token,
                    quality: _pictureOptions.Product.Medium.Quality);
                await UploadProductPictureAsync(image, entity.Producer.Id, entity.Id, imageId, PictureSize.SMALL,
                    _pictureOptions.Product.Small.Width, _pictureOptions.Product.Small.Height, token,
                    quality: _pictureOptions.Product.Small.Quality);
            }

            if (originalBytes != null)
                using (Image image = Image.Load(originalBytes))
                    await UploadProductPictureAsync(image, entity.Producer.Id, entity.Id, imageId,
                        PictureSize.ORIGINAL, token);

            return Success(
                $"{_storageOptions.ContentScheme}://{_storageOptions.ContentHostname}/{_storageOptions.Containers.Pictures}/{ProductExtensions.GetPictureUrl(entity.Producer.Id, entity.Id, imageId)}");
        }

        private string GetDefaultProductPicture(IEnumerable<Tag> tags)
        {
            var category = tags.FirstOrDefault(t => t.Kind == TagKind.Category);
            return category?.Picture;
        }

        private async Task<Result<string>> UploadTagPictureAsync(Image image, Guid tagId, string filename, string size,
            int width, int height, CancellationToken token, ResizeMode mode = ResizeMode.Max, int quality = 100)
        {
            using (var blobStream = new MemoryStream())
            {
                image.Clone(context => context.Resize(new ResizeOptions
                {
                    Mode = mode,
                    Size = new Size(width, height),
                    Compand = true,
                    Sampler = KnownResamplers.Lanczos3
                })).Save(blobStream, new JpegEncoder {Quality = quality, Subsample = JpegSubsample.Ratio444});

                return await _blobService.UploadTagPictureAsync(tagId, filename, size, blobStream.ToArray(), token);
            }
        }

        private async Task<Result<string>> UploadProductPictureAsync(Image image, Guid userId, Guid productId,
            string filename, string size, int width, int height, CancellationToken token,
            ResizeMode mode = ResizeMode.Max, int quality = 100)
        {
            using (var blobStream = new MemoryStream())
            {
                image.Clone(context => context.Resize(new ResizeOptions
                {
                    Mode = mode,
                    Size = new Size(width, height),
                    Compand = true,
                    Sampler = KnownResamplers.Lanczos3
                })).Save(blobStream, new JpegEncoder {Quality = quality, Subsample = JpegSubsample.Ratio444});

                return await _blobService.UploadProductPictureAsync(userId, productId, filename, size,
                    blobStream.ToArray(), token);
            }
        }

        private async Task<Result<string>> UploadProductPictureAsync(Image image, Guid userId, Guid productId,
            string filename, string size, CancellationToken token)
        {
            using (var blobStream = new MemoryStream())
            {
                image.Save(blobStream, new JpegEncoder {Quality = 100, Subsample = JpegSubsample.Ratio444});
                return await _blobService.UploadProductPictureAsync(userId, productId, filename, size,
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