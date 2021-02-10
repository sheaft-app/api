using Sheaft.Domain.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sheaft.Core;
using Newtonsoft.Json;
using Sheaft.Application.Interop;

namespace Sheaft.Application.Commands
{
    public class UpdateRegionCommand : Command<bool>
    {
        [JsonConstructor]
        public UpdateRegionCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public int? RequiredProducers { get; set; }
    }
    
    public class UpdateRegionCommandHandler : CommandsHandler,
        IRequestHandler<UpdateRegionCommand, Result<bool>>
    {
        public UpdateRegionCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<UpdateRegionCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result<bool>> Handle(UpdateRegionCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var entity = await _context.Regions.SingleOrDefaultAsync(c => c.Id == request.Id, token);
                entity.SetName(request.Name);
                entity.SetRequiredProducers(request.RequiredProducers);

                await _context.SaveChangesAsync(token);
                return Ok(true);
            });
        }
    }
}
