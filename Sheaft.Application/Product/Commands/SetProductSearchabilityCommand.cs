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
    public class SetProductSearchabilityCommand : Command<bool>
    {
        [JsonConstructor]
        public SetProductSearchabilityCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
        public bool VisibleToStores { get; set; }
        public bool VisibleToConsumers { get; set; }
    }
    
    public class SetProductSearchabilityCommandHandler : CommandsHandler,
        IRequestHandler<SetProductSearchabilityCommand, Result<bool>>
    {
        private readonly IBlobService _blobService;

        public SetProductSearchabilityCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IBlobService blobService,
            ILogger<SetProductSearchabilityCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _blobService = blobService;
        }
        public async Task<Result<bool>> Handle(SetProductSearchabilityCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var entity = await _context.GetByIdAsync<Product>(request.Id, token);
                entity.SetConsumerVisibility(request.VisibleToConsumers);
                entity.SetStoreVisibility(request.VisibleToStores);

                await _context.SaveChangesAsync(token);
                return Ok(true);
            });
        }
    }
}
