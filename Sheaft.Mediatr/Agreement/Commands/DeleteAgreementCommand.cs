using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Core.Enums;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;
using Sheaft.Options;

namespace Sheaft.Mediatr.Agreement.Commands
{
    public class DeleteAgreementCommand : Command
    {
        protected DeleteAgreementCommand()
        {
        }

        [JsonConstructor]
        public DeleteAgreementCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid AgreementId { get; set; }
    }

    public class DeleteAgreementCommandsHandler : CommandsHandler,
        IRequestHandler<DeleteAgreementCommand, Result>
    {
        private readonly RoleOptions _roleOptions;

        public DeleteAgreementCommandsHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IOptionsSnapshot<RoleOptions> roleOptions,
            ILogger<DeleteAgreementCommandsHandler> logger)
            : base(mediatr, context, logger)
        {
            _roleOptions = roleOptions.Value;
        }

        public async Task<Result> Handle(DeleteAgreementCommand request, CancellationToken token)
        {
            var entity = await _context.Agreements.SingleAsync(e => e.Id == request.AgreementId, token);
            if (request.RequestUser.IsInRole(_roleOptions.Producer.Value) &&
                entity.Delivery.Producer.Id != request.RequestUser.Id)
                return Failure(MessageKind.Forbidden);

            if (request.RequestUser.IsInRole(_roleOptions.Store.Value) && entity.Store.Id != request.RequestUser.Id)
                return Failure(MessageKind.Forbidden);

            _context.Remove(entity);

            var quickOrders = await _context.QuickOrders
                .Where(qo => qo.Products.Any(qop => qop.CatalogProduct.Catalog.Id == entity.Catalog.Id) &&
                             qo.User.Id == entity.Store.Id)
                .ToListAsync(token);

            foreach (var quickOrder in quickOrders)
            {
                var products = quickOrder.Products.Where(p => p.CatalogProduct.Catalog.Id == entity.Catalog.Id).ToList();
                foreach (var product in products)
                    quickOrder.RemoveProduct(product);
            }

            await _context.SaveChangesAsync(token);

            return Success();
        }
    }
}