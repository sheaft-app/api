using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Models;
using Sheaft.Core;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Application.Interfaces.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Sheaft.Core.Enums;
using Sheaft.Core.Exceptions;
using Sheaft.Mediatr.Producer.Commands;

namespace Sheaft.Mediatr.Delivery.Commands
{
    public class CompleteDeliveryCommand : Command
    {
        protected CompleteDeliveryCommand()
        {
        }

        [JsonConstructor]
        public CompleteDeliveryCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid DeliveryId { get; set; }
        public string ReceptionedBy { get; set; }
        public string Comment { get; set; }
        public IEnumerable<ReturnedProductDto> ReturnedProducts { get; set; }
        public IEnumerable<ReturnedReturnableDto> ReturnedReturnables { get; set; }
    }

    public class CompleteDeliveryCommandHandler : CommandsHandler,
        IRequestHandler<CompleteDeliveryCommand, Result>
    {
        public CompleteDeliveryCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<CompleteDeliveryCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(CompleteDeliveryCommand request, CancellationToken token)
        {
            var delivery = await _context.Set<Domain.Delivery>()
                .Include(d => d.Products)
                .SingleOrDefaultAsync(c => c.Id == request.DeliveryId, token);

            if (delivery == null)
                return Failure(MessageKind.NotFound);

            var returnedProductIds = request.ReturnedProducts.Select(p => p.ProductId);
            var products = delivery.Products.Where(p =>
                    returnedProductIds.Contains(p.ProductId) && p.RowKind == ModificationKind.Deliver)
                .ToList();

            if (products.Count() != returnedProductIds.Count())
                throw SheaftException.NotFound();

            var returnedProducts = products.Select(p =>
            {
                var product = request.ReturnedProducts.Single(pr => pr.ProductId == p.ProductId);
                return new Tuple<DeliveryProduct, int, ModificationKind>(p, product.Quantity, product.Kind);
            }).ToList();

            var returnableIds = request.ReturnedReturnables.Select(r => r.ReturnableId);
            var returnables = await _context.Returnables
                .Where(r => returnableIds.Contains(r.Id))
                .ToListAsync(token);

            if (returnables.Count() != returnableIds.Count())
                throw SheaftException.NotFound();

            var returnedReturnables = returnables.Select(r =>
                    new KeyValuePair<Domain.Returnable, int>(r,
                        request.ReturnedReturnables.Single(re => re.ReturnableId == r.Id).Quantity))
                .ToList();

            delivery.CompleteDelivery(returnedProducts, returnedReturnables, request.ReceptionedBy, request.Comment);

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}