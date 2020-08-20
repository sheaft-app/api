using Sheaft.Infrastructure.Interop;
using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Core;
using Sheaft.Application.Commands;
using Sheaft.Domain.Models;
using Microsoft.Extensions.Logging;

namespace Sheaft.Application.Handlers
{
    public class PackagingCommandsHandler : CommandsHandler,
        IRequestHandler<CreatePackagingCommand, CommandResult<Guid>>,
        IRequestHandler<UpdatePackagingCommand, CommandResult<bool>>,
        IRequestHandler<DeletePackagingCommand, CommandResult<bool>>,
        IRequestHandler<RestorePackagingCommand, CommandResult<bool>>
    {
        private readonly IAppDbContext _context;

        public PackagingCommandsHandler(
            IAppDbContext context,
            ILogger<PackagingCommandsHandler> logger) : base(logger)
        {
            _context = context;
        }

        public async Task<CommandResult<Guid>> Handle(CreatePackagingCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var company = await _context.GetByIdAsync<Company>(request.RequestUser.CompanyId, token);
                var packaging = new Packaging(Guid.NewGuid(), company, request.Name, request.WholeSalePrice, request.Vat, request.Description);

                await _context.AddAsync(packaging, token);
                await _context.SaveChangesAsync(token);

                return CreatedResult(packaging.Id);
            });
        }

        public async Task<CommandResult<bool>> Handle(UpdatePackagingCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var packaging = await _context.GetByIdAsync<Packaging>(request.Id, token);

                packaging.SetName(request.Name);
                packaging.SetDescription(request.Description);
                packaging.SetWholeSalePrice(request.WholeSalePrice);
                packaging.SetVat(request.Vat);

                return OkResult(await _context.SaveChangesAsync(token) > 0);
            });
        }

        public async Task<CommandResult<bool>> Handle(DeletePackagingCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.GetByIdAsync<Packaging>(request.Id, token);

                _context.Remove(entity);

                return OkResult(await _context.SaveChangesAsync(token) > 0);
            });
        }

        public async Task<CommandResult<bool>> Handle(RestorePackagingCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.GetByIdAsync<Packaging>(request.Id, token);
                entity.Restore();

                _context.Remove(entity);
                return OkResult(await _context.SaveChangesAsync(token) > 0);
            });
        }
    }
}