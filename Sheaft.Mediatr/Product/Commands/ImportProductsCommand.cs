using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces.Factories;
using Sheaft.Application.Interfaces.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Core.Enums;
using Sheaft.Domain;
using Sheaft.Domain.Events.Product;
using Sheaft.Mediatr.Job.Commands;
using Sheaft.Mediatr.Producer.Commands;

namespace Sheaft.Mediatr.Product.Commands
{
    public class ImportProductsCommand : Command
    {
        protected ImportProductsCommand()
        {
            
        }
        [JsonConstructor]
        public ImportProductsCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid JobId { get; set; }
        public bool NotifyOnUpdates { get; set; } = true;
    }

    public class ImportProductsCommandHandler : CommandsHandler,
        IRequestHandler<ImportProductsCommand, Result>
    {
        private readonly IBlobService _blobService;
        private readonly IMapper _mapper;
        private readonly IProductsImporterFactory _productsImporterFactory;

        public ImportProductsCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IBlobService blobService,
            IMapper mapper,
            IProductsImporterFactory productsImporterFactory,
            ILogger<ImportProductsCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _blobService = blobService;
            _mapper = mapper;
            _productsImporterFactory = productsImporterFactory;
        }

        public async Task<Result> Handle(ImportProductsCommand request, CancellationToken token)
        {
            var job = await _context.Jobs.SingleAsync(e => e.Id == request.JobId, token);

            try
            {
                job.CompleteJob(request.NotifyOnUpdates ? new ProductImportProcessingEvent(job.Id) : null);
                await _context.SaveChangesAsync(token);

                var fileDataResult = await _blobService.DownloadImportProductsFileAsync(job.UserId, job.Id, token);
                if (!fileDataResult.Succeeded)
                    return fileDataResult;

                var productsImporter = await _productsImporterFactory.GetImporterAsync(request.RequestUser, token);
                
                var tags = await _context.Tags.ToListAsync(token);
                var productsResult = await productsImporter.ImportAsync(fileDataResult.Data, tags.Select(t => new KeyValuePair<Guid, string>(t.Id, t.Name)), token);
                if (!productsResult.Succeeded)
                    return productsResult;

                using (var transaction = await _context.BeginTransactionAsync(token))
                {
                    Result productResult = null;
                    foreach (var product in productsResult.Data)
                    {
                        var command = _mapper.Map(product, new CreateProductCommand(request.RequestUser){SkipUpdateProducerTags = true});
                        productResult = await _mediatr.Process(command, token);
                        if (!productResult.Succeeded)
                            break;
                    }

                    if (productResult is {Succeeded: false})
                        return productResult;
                    
                    await _context.SaveChangesAsync(token);
                    await transaction.CommitAsync(token);
                }

                job.CompleteJob(request.NotifyOnUpdates ? new ProductImportSucceededEvent(job.Id) : null);
                await _context.SaveChangesAsync(token);
                
                _mediatr.Post(new UpdateProducerTagsCommand(request.RequestUser)
                    {ProducerId = job.UserId});

                return Success();
            }
            catch (Exception e)
            {
                job.FailJob(e.Message, request.NotifyOnUpdates ? new ProductImportFailedEvent(job.Id) : null);
                await _context.SaveChangesAsync(token);
                
                return Failure(MessageKind.JobFailure);
            }
        }
    }
}