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
    public class SetProductsAvailabilityCommand : Command<bool>
    {
        [JsonConstructor]
        public SetProductsAvailabilityCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public IEnumerable<Guid> Ids { get; set; }
        public bool Available { get; set; }
    }
    
    public class SetProductsAvailabilityCommandHandler : CommandsHandler,
        IRequestHandler<SetProductsAvailabilityCommand, Result<bool>>
    {
        private readonly IBlobService _blobService;

        public SetProductsAvailabilityCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IBlobService blobService,
            ILogger<SetProductsAvailabilityCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _blobService = blobService;
        }
        public async Task<Result<bool>> Handle(SetProductsAvailabilityCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                using (var transaction = await _context.BeginTransactionAsync(token))
                {
                    foreach (var id in request.Ids)
                    {
                        var result = await _mediatr.Process(new SetProductAvailabilityCommand(request.RequestUser) { Id = id, Available = request.Available }, token);
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
