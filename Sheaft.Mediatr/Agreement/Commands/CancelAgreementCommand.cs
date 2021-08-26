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
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Core;
using Sheaft.Core.Options;
using Sheaft.Domain;

namespace Sheaft.Mediatr.Agreement.Commands
{
    public class CancelAgreementCommand : Command
    {
        protected CancelAgreementCommand()
        {
        }

        [JsonConstructor]
        public CancelAgreementCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid AgreementId { get; set; }
        public string Reason { get; set; }
    }

    public class CancelAgreementCommandsHandler : CommandsHandler,
        IRequestHandler<CancelAgreementCommand, Result>
    {
        private readonly RoleOptions _roleOptions;

        public CancelAgreementCommandsHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IOptionsSnapshot<RoleOptions> roleOptions,
            ILogger<CancelAgreementCommandsHandler> logger)
            : base(mediatr, context, logger)
        {
            _roleOptions = roleOptions.Value;
        }

        public async Task<Result> Handle(CancelAgreementCommand request, CancellationToken token)
        {
            var entity = await _context.Agreements.SingleAsync(e => e.Id == request.AgreementId, token);
            if(request.RequestUser.IsInRole(_roleOptions.Producer.Value) && entity.ProducerId != request.RequestUser.Id)
                return Failure("Vous n'êtes pas autorisé à accéder à cette ressource.");
            
            if(request.RequestUser.IsInRole(_roleOptions.Store.Value) && entity.StoreId != request.RequestUser.Id)
                return Failure("Vous n'êtes pas autorisé à accéder à cette ressource.");
            
            var currentUser = await _context.Users.SingleAsync(c => c.Id == request.RequestUser.Id, token);
            entity.CancelAgreement(request.Reason, currentUser.Kind);

            var quickOrders = await _context.QuickOrders
                .Where(qo => qo.Products.Any(qop => qop.CatalogProduct.CatalogId == entity.CatalogId) &&
                             qo.UserId == entity.StoreId)
                .ToListAsync(token);

            foreach (var quickOrder in quickOrders)
            {
                var products = quickOrder.Products.Where(p => p.CatalogProduct.CatalogId == entity.CatalogId).ToList();
                _context.RemoveRange(products);
            }

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}