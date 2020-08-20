using Sheaft.Infrastructure.Interop;
using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Core;
using Sheaft.Application.Commands;
using Sheaft.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using Sheaft.Exceptions;
using System.Net.Http;
using Sheaft.Services.Interop;
using Sheaft.Options;
using SixLabors.ImageSharp.Processing;
using System.IO;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace Sheaft.Application.Handlers
{
    public class TagCommandsHandler : CommandsHandler,
        IRequestHandler<CreateTagCommand, CommandResult<Guid>>,
        IRequestHandler<UpdateTagCommand, CommandResult<bool>>,
        IRequestHandler<DeleteTagCommand, CommandResult<bool>>,
        IRequestHandler<RestoreTagCommand, CommandResult<bool>>
    {
        private readonly IAppDbContext _context;
        private readonly IBlobService _blobsService;
        private readonly StorageOptions _storageOptions;
        private readonly HttpClient _httpClient;

        public TagCommandsHandler(
            IAppDbContext context,
            IOptionsSnapshot<StorageOptions> storageOptions,
            IHttpClientFactory httpClientFactory,
            IBlobService blobsService,
            ILogger<TagCommandsHandler> logger) : base(logger)
        {
            _blobsService = blobsService;
            _httpClient = httpClientFactory.CreateClient("picture");
            _storageOptions = storageOptions.Value;
            _context = context;
        }

        public async Task<CommandResult<Guid>> Handle(CreateTagCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = new Tag(Guid.NewGuid(), request.Kind, request.Name, request.Description, request.Image);

                await _context.AddAsync(entity, token);
                await _context.SaveChangesAsync(token);

                return CreatedResult(entity.Id);
            });
        }

        public async Task<CommandResult<bool>> Handle(UpdateTagCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.GetByIdAsync<Tag>(request.Id, token);

                entity.SetName(request.Name);
                entity.SetDescription(request.Description);
                entity.SetKind(request.Kind);

                var image = await HandleImageAsync(entity.Id, request.Image, token);
                entity.SetImage(image);

                _context.Update(entity);

                return OkResult(await _context.SaveChangesAsync(token) > 0);
            });
        }

        public async Task<CommandResult<bool>> Handle(DeleteTagCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.GetByIdAsync<Tag>(request.Id, token);

                _context.Remove(entity);
                var results = await _context.SaveChangesAsync(token);

                return OkResult(results > 0);
            });
        }

        public async Task<CommandResult<bool>> Handle(RestoreTagCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.Tags.SingleOrDefaultAsync(a => a.Id == request.Id && a.RemovedOn.HasValue, token);
                entity.Restore();

                _context.Update(entity);
                return OkResult(await _context.SaveChangesAsync(token) > 0);
            });
        }

        private async Task<string> HandleImageAsync(Guid id, string picture, CancellationToken token)
        {
            if (string.IsNullOrWhiteSpace(picture))
                return null;

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
                return picture;

            var imageId = Guid.NewGuid().ToString("N");

            using (var image = Image.Load(bytes))
            {
                using (var blobStream = new MemoryStream())
                {
                    image.Clone(context => context.Resize(new ResizeOptions
                    {
                        Mode = ResizeMode.Max,
                        Size = new Size(64, 64)
                    })).Save(blobStream, new JpegEncoder { Quality = 100 });

                    var compImage = await _blobsService.UploadTagPictureAsync(id, blobStream, token);
                    if (!compImage.Success)
                        throw compImage.Exception ?? new BadRequestException();

                    return compImage.Result;
                }
            }
        }
    }
}