using Sheaft.Application.Interop;
using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Core;
using Sheaft.Application.Commands;
using Sheaft.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Sheaft.Application.Handlers
{
    public class TagCommandsHandler : ResultsHandler,
        IRequestHandler<CreateTagCommand, Result<Guid>>,
        IRequestHandler<UpdateTagCommand, Result<bool>>,
        IRequestHandler<DeleteTagCommand, Result<bool>>,
        IRequestHandler<RestoreTagCommand, Result<bool>>
    {
        public TagCommandsHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<TagCommandsHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result<Guid>> Handle(CreateTagCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var entity = new Tag(Guid.NewGuid(), request.Kind, request.Name, request.Description, request.Picture);
                entity.SetAvailable(request.Available);

                await _context.AddAsync(entity, token);
                await _context.SaveChangesAsync(token);

                return Created(entity.Id);
            });
        }

        public async Task<Result<bool>> Handle(UpdateTagCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var entity = await _context.GetByIdAsync<Tag>(request.Id, token);

                entity.SetName(request.Name);
                entity.SetDescription(request.Description);
                entity.SetKind(request.Kind);
                entity.SetAvailable(request.Available);

                await _context.SaveChangesAsync(token);

                var imageResult = await _mediatr.Process(new UpdateTagPictureCommand(request.RequestUser) { TagId = entity.Id, Picture = request.Picture }, token);
                if (!imageResult.Success)
                    return Failed<bool>(imageResult.Exception);

                var iconResult = await _mediatr.Process(new UpdateTagIconCommand(request.RequestUser) { TagId = entity.Id, Icon = request.Icon }, token);
                if (!iconResult.Success)
                    return Failed<bool>(iconResult.Exception);

                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(DeleteTagCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var entity = await _context.GetByIdAsync<Tag>(request.Id, token);
                _context.Remove(entity);

                await _context.SaveChangesAsync(token);
                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(RestoreTagCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var entity = await _context.Tags.SingleOrDefaultAsync(a => a.Id == request.Id && a.RemovedOn.HasValue, token);
                _context.Restore(entity);

                await _context.SaveChangesAsync(token);
                return Ok(true);
            });
        }
    }
}