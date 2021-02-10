using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interop;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class RestoreProductCommand : Command<bool>
    {
        [JsonConstructor]
        public RestoreProductCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
    }
    
    public class RestoreProductCommandHandler : CommandsHandler,
        IRequestHandler<RestoreProductCommand, Result<bool>>
    {
        private readonly IBlobService _blobService;

        public RestoreProductCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IBlobService blobService,
            ILogger<RestoreProductCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _blobService = blobService;
        }

        public async Task<Result<bool>> Handle(RestoreProductCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var entity = await _context.Products.SingleOrDefaultAsync(a => a.Id == request.Id && a.RemovedOn.HasValue, token);
                _context.Restore(entity);

                await _context.SaveChangesAsync(token);
                return Ok(true);
            });
        }
    }
}
