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
    public class SetProductsSearchabilityCommand : Command<bool>
    {
        [JsonConstructor]
        public SetProductsSearchabilityCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public IEnumerable<Guid> Ids { get; set; }
        public bool VisibleToStores { get; set; }
        public bool VisibleToConsumers { get; set; }
    }
    
    public class SetProductsSearchabilityCommandHandler : CommandsHandler,
        IRequestHandler<SetProductsSearchabilityCommand, Result<bool>>
    {
        private readonly IBlobService _blobService;

        public SetProductsSearchabilityCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IBlobService blobService,
            ILogger<SetProductsSearchabilityCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _blobService = blobService;
        }
        public async Task<Result<bool>> Handle(SetProductsSearchabilityCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                using (var transaction = await _context.BeginTransactionAsync(token))
                {
                    foreach (var id in request.Ids)
                    {
                        var result = await _mediatr.Process(new SetProductSearchabilityCommand(request.RequestUser) { Id = id, VisibleToStores = request.VisibleToStores, VisibleToConsumers = request.VisibleToConsumers }, token);
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
