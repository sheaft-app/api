using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interop;
using Sheaft.Core;
using Sheaft.Domain.Enums;
using Sheaft.Domain.Models;

namespace Sheaft.Application.Commands
{
    public class QueueImportProductsCommand : Command<Guid>
    {
        [JsonConstructor]
        public QueueImportProductsCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
        public string FileName { get; set; }
        public byte[] FileStream { get; set; }
        public bool NotifyOnUpdates { get; set; } = true;
    }
    
    public class QueueImportProductsCommandHandler : CommandsHandler,
        IRequestHandler<QueueImportProductsCommand, Result<Guid>>
    {
        private readonly IBlobService _blobService;

        public QueueImportProductsCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IBlobService blobService,
            ILogger<QueueImportProductsCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _blobService = blobService;
        }
        public async Task<Result<Guid>> Handle(QueueImportProductsCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var producer = await _context.GetByIdAsync<Producer>(request.RequestUser.Id, token);
                var entity = new Job(Guid.NewGuid(), JobKind.ImportProducts, $"Import produits", producer);

                var response = await _blobService.UploadImportProductsFileAsync(producer.Id, entity.Id, request.FileStream, token);
                if (!response.Success)
                    return Failed<Guid>(response.Exception);

                await _context.AddAsync(entity, token);
                await _context.SaveChangesAsync(token);

                _mediatr.Post(new ImportProductsCommand(request.RequestUser) { Id = entity.Id, NotifyOnUpdates = request.NotifyOnUpdates });
                return Created(entity.Id);
            });
        }
    }
}
