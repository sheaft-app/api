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
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
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
                var startResult =
                    await _mediatr.Process(new StartJobCommand(request.RequestUser) {JobId = job.Id}, token);
                if (!startResult.Succeeded)
                    return startResult;

                if (request.NotifyOnUpdates)
                    _mediatr.Post(new ProductImportProcessingEvent(job.Id));

                var fileDataResult = await _blobService.DownloadImportProductsFileAsync(job.User.Id, job.Id, token);
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

                _mediatr.Post(new UpdateProducerTagsCommand(request.RequestUser)
                    {ProducerId = job.User.Id});

                var result = await _mediatr.Process(new CompleteJobCommand(request.RequestUser) {JobId = job.Id}, token);
                if (!result.Succeeded)
                    return result;

                if (request.NotifyOnUpdates)
                    _mediatr.Post(new ProductImportSucceededEvent(job.Id));

                return result;
            }
            catch (Exception e)
            {
                if (request.NotifyOnUpdates)
                    _mediatr.Post(new ProductImportFailedEvent(job.Id));

                return await _mediatr.Process(new FailJobCommand(request.RequestUser) {JobId = job.Id, Reason = e.Message},
                    token);
            }
        }
    }
}