using Sheaft.Application.Interop;
using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Core;
using Sheaft.Application.Commands;
using Sheaft.Domain.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Sheaft.Domain.Enums;

namespace Sheaft.Application.Handlers
{
    public class ReturnableCommandsHandler : ResultsHandler,
        IRequestHandler<CreateReturnableCommand, Result<Guid>>,
        IRequestHandler<UpdateReturnableCommand, Result<bool>>,
        IRequestHandler<DeleteReturnableCommand, Result<bool>>,
        IRequestHandler<RestoreReturnableCommand, Result<bool>>
    {
        public ReturnableCommandsHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<ReturnableCommandsHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result<Guid>> Handle(CreateReturnableCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var producer = await _context.GetByIdAsync<Producer>(request.RequestUser.Id, token);
                var returnable = new Returnable(Guid.NewGuid(), ReturnableKind.Container, producer, request.Name, request.WholeSalePrice, request.Vat, request.Description);

                await _context.AddAsync(returnable, token);
                await _context.SaveChangesAsync(token);

                return Created(returnable.Id);
            });
        }

        public async Task<Result<bool>> Handle(UpdateReturnableCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.GetByIdAsync<Returnable>(request.Id, token);

                entity.SetName(request.Name);
                entity.SetDescription(request.Description);
                entity.SetWholeSalePrice(request.WholeSalePrice);
                entity.SetVat(request.Vat);

                _context.Update(entity);

                return Ok(await _context.SaveChangesAsync(token) > 0);
            });
        }

        public async Task<Result<bool>> Handle(DeleteReturnableCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.GetByIdAsync<Returnable>(request.Id, token);
                _context.Remove(entity);

                var results = await _context.SaveChangesAsync(token);

                return Ok(results > 0);
            });
        }

        public async Task<Result<bool>> Handle(RestoreReturnableCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.Returnables.SingleOrDefaultAsync(a => a.Id == request.Id && a.RemovedOn.HasValue, token);                
                _context.Restore(entity);

                return Ok(await _context.SaveChangesAsync(token) > 0);
            });
        }
    }
}