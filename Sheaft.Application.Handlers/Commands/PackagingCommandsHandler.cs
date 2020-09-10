using Sheaft.Infrastructure.Interop;
using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Core;
using Sheaft.Application.Commands;
using Sheaft.Domain.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace Sheaft.Application.Handlers
{
    public class PackagingCommandsHandler : CommandsHandler,
        IRequestHandler<CreatePackagingCommand, Result<Guid>>,
        IRequestHandler<UpdatePackagingCommand, Result<bool>>,
        IRequestHandler<DeletePackagingCommand, Result<bool>>,
        IRequestHandler<RestorePackagingCommand, Result<bool>>
    {
        private readonly IAppDbContext _context;

        public PackagingCommandsHandler(
            IAppDbContext context,
            ILogger<PackagingCommandsHandler> logger) : base(logger)
        {
            _context = context;
        }

        public async Task<Result<Guid>> Handle(CreatePackagingCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var producer = await _context.GetByIdAsync<Producer>(request.RequestUser.Id, token);
                var packaging = new Packaging(Guid.NewGuid(), producer, request.Name, request.WholeSalePrice, request.Vat, request.Description);

                await _context.AddAsync(packaging, token);
                await _context.SaveChangesAsync(token);

                return Created(packaging.Id);
            });
        }

        public async Task<Result<bool>> Handle(UpdatePackagingCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.GetByIdAsync<Packaging>(request.Id, token);

                entity.SetName(request.Name);
                entity.SetDescription(request.Description);
                entity.SetWholeSalePrice(request.WholeSalePrice);
                entity.SetVat(request.Vat);

                _context.Update(entity);

                return Ok(await _context.SaveChangesAsync(token) > 0);
            });
        }

        public async Task<Result<bool>> Handle(DeletePackagingCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.GetByIdAsync<Packaging>(request.Id, token);
                _context.Remove(entity);

                var results = await _context.SaveChangesAsync(token);

                using (var transaction = await _context.Database.BeginTransactionAsync(token))
                {
                    await _context.Database.ExecuteSqlRawAsync($"UPDATE [dbo].[Products] SET [PackagingId] = NULL WHERE [PackagingId] = '{entity.Id}';", token);
                    await transaction.CommitAsync(token);
                }

                return Ok(results > 0);
            });
        }

        public async Task<Result<bool>> Handle(RestorePackagingCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.Packagings.SingleOrDefaultAsync(a => a.Id == request.Id && a.RemovedOn.HasValue, token);                
                _context.Restore(entity);

                return Ok(await _context.SaveChangesAsync(token) > 0);
            });
        }
    }
}