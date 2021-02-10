using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interop;
using Sheaft.Core;
using Sheaft.Domain.Models;

namespace Sheaft.Application.Commands
{
    public class SetProductAvailabilityCommand : Command<bool>
    {
        [JsonConstructor]
        public SetProductAvailabilityCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
        public bool Available { get; set; }
    }
    
    public class SetProductAvailabilityCommandHandler : CommandsHandler,
        IRequestHandler<SetProductAvailabilityCommand, Result<bool>>
    {
        private readonly IBlobService _blobService;

        public SetProductAvailabilityCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IBlobService blobService,
            ILogger<SetProductAvailabilityCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _blobService = blobService;
        }

        public async Task<Result<bool>> Handle(SetProductAvailabilityCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var entity = await _context.GetByIdAsync<Product>(request.Id, token);
                entity.SetAvailable(request.Available);

                await _context.SaveChangesAsync(token);
                return Ok(true);
            });
        }
    }
}
