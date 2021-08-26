using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Core;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Mediatr.Product.Commands
{
    public class QueueImportProductsCommand : Command<Guid>
    {
        protected QueueImportProductsCommand()
        {
            
        }
        [JsonConstructor]
        public QueueImportProductsCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid ProducerId { get; set; }
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
            var producer = await _context.Producers.SingleAsync(e => e.Id == request.ProducerId, token);
            
            var command = new ImportProductsCommand(request.RequestUser)
                {JobId = Guid.NewGuid(), NotifyOnUpdates = request.NotifyOnUpdates};
            
            var entity = new Domain.Job(command.JobId, JobKind.ImportProducts, $"Import produits", producer, command);

            var response =
                await _blobService.UploadImportProductsFileAsync(request.ProducerId, entity.Id, request.FileStream, token);
            if (!response.Succeeded)
                return Failure<Guid>(response);

            await _context.AddAsync(entity, token);
            await _context.SaveChangesAsync(token);

            _mediatr.Post(command);
            return Success(entity.Id);
        }
    }
}