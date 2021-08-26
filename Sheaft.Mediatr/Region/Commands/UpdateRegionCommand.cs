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

namespace Sheaft.Mediatr.Region.Commands
{
    public class UpdateRegionCommand : Command
    {
        protected UpdateRegionCommand()
        {
            
        }
        [JsonConstructor]
        public UpdateRegionCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid RegionId { get; set; }
        public string Name { get; set; }
        public int? RequiredProducers { get; set; }
    }

    public class UpdateRegionCommandHandler : CommandsHandler,
        IRequestHandler<UpdateRegionCommand, Result>
    {
        public UpdateRegionCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<UpdateRegionCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(UpdateRegionCommand request, CancellationToken token)
        {
            var entity = await _context.Regions.SingleOrDefaultAsync(c => c.Id == request.RegionId, token);
            entity.SetName(request.Name);
            entity.SetRequiredProducers(request.RequiredProducers);

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}