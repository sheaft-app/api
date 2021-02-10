using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interop;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class DeleteProductsCommand : Command<bool>
    {
        [JsonConstructor]
        public DeleteProductsCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public IEnumerable<Guid> Ids { get; set; }
    }
    
    public class DeleteProductsCommandHandler : CommandsHandler,
        IRequestHandler<DeleteProductsCommand, Result<bool>>
    {
        private readonly IBlobService _blobService;

        public DeleteProductsCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IBlobService blobService,
            ILogger<DeleteProductsCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _blobService = blobService;
        }
        
        public async Task<Result<bool>> Handle(DeleteProductsCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                using (var transaction = await _context.BeginTransactionAsync(token))
                {
                    foreach (var id in request.Ids)
                    {
                        var result = await _mediatr.Process(new DeleteProductCommand(request.RequestUser) { Id = id }, token);
                        if (!result.Success)
                            return Failed<bool>(result.Exception);
                    }

                    await transaction.CommitAsync(token);
                    return Ok(true);
                }
            });
        }
    }
}
